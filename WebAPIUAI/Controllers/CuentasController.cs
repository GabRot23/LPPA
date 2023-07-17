using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPIUAI.Data;
using WebAPIUAI.DTOs;
using WebAPIUAI.Models;
using WebAPIUAI.Services;

namespace WebAPIUAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentasController : CustomBaseController
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        private List<string> errors;

        public CuentasController(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            IMapper mapper
            ) : base(context, mapper)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.context = context;
            this.mapper = mapper;
            errors = new List<string>();
        }

        [HttpPost("signup")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var userExiste = await userManager.FindByEmailAsync(model.Email);
            if (userExiste != null)
            {
                errors.Add("El usuario ya existe.");
                return BadRequest(
                        new { errors = errors }
                );
            }
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return await ConstruirToken(model);
            }
            else
            {
                errors.Add("Error al crear el usuario.");
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
                return BadRequest(
                    new { errors = errors }
                );
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                //guardar el login

                return await ConstruirToken(model);
            }
            else
            {
                errors.Add("Error al iniciar sesión.");

                return BadRequest(
                    new { errors = errors }
                );
            }
        }

        [HttpGet("RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserToken>> Renovar()
        {
            var userInfo = new UserInfo()
            {
                Email = HttpContext.User.Identity.Name
            };

            var tokenNuevo = await ConstruirToken(userInfo);

            var refreshToken = new RefreshToken()
            {
                UsuarioId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value,
                Token = tokenNuevo.Token,
                FechaCreacion = DateTime.Now,
                FechaExpiracion = tokenNuevo.Expiration,
            };

            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();

            return tokenNuevo;
        }

        private async Task<UserToken> ConstruirToken(UserInfo userInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email)
            };

            var identityUser = await userManager.FindByEmailAsync(userInfo.Email);

            claims.Add(new Claim(ClaimTypes.NameIdentifier, identityUser.Id));

            var claimsDB = await userManager.GetClaimsAsync(identityUser);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.Now.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        [HttpGet("Users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<UserDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Users.AsQueryable();
            queryable = queryable.OrderBy(x => x.Email);
            return await Get<IdentityUser, UserDTO>(paginacionDTO);
        }

        [HttpGet("Roles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<string>>> GetRoles()
        {
            return await context.Roles.Select(x => x.Name).ToListAsync();
        }

        //Get Claims de un usuario
        [HttpGet("UserRoles/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<UserClaimsDTO>> GetUserClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            var claims = await userManager.GetClaimsAsync(user);

            var claimsDTO = new List<ClaimDTO>();

            foreach (Claim claim in claims)
            {
                claimsDTO.Add(new ClaimDTO() { ClaimType = claim.Type, ClaimValue = claim.Value });
            }

            var userDTO = new UserClaimsDTO()
            {
                Id = user.Id,
                Email = user.Email,
                Claims = claimsDTO,
            };

            return userDTO;
        }


        //Post para crear roles
        [HttpPost("CrearRol")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> CrearRol([FromBody] CrearRolDTO crearRolDTO)
        {
            var role = new IdentityRole { Name = crearRolDTO.Nombre, NormalizedName = crearRolDTO.Nombre.ToUpper() };
            await context.Roles.AddAsync(role);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("AsignarRol")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> AsignarRol(EditarUserRolDTO editarRolDTO)
        {
            var user = await userManager.FindByIdAsync(editarRolDTO.UserId);
            if (user == null)
            {
                errors.Add("El usuario no existe.");
            }

            //Validar si el rol existe
            var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == editarRolDTO.RoleName);
            if (role == null)
            {
                errors.Add("El rol no existe.");
            }

            //Validar si el usuario ya tiene el rol
            var userHasRole = await userManager.IsInRoleAsync(user, editarRolDTO.RoleName);
            var userHasClaim = await userManager.GetClaimsAsync(user);
            foreach (var claim in userHasClaim)
            {
                if (claim.Type == ClaimTypes.Role && claim.Value == editarRolDTO.RoleName)
                {
                    userHasRole = true;
                }
            }

            if (userHasRole)
            {
                errors.Add("El usuario ya posee el rol asignado.");
            }

            if (errors.Count > 0)
            {
                return BadRequest(
                    new { errors = errors }
                );
            }
            else
            {
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));
                return Ok("Se asigno el rol correctamente.");
            }
        }

        [HttpPost("RemoverRol")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> RemoverRol(EditarUserRolDTO editarRolDTO)
        {
            var user = await userManager.FindByIdAsync(editarRolDTO.UserId);
            if (user == null)
            {
                errors.Add("El usuario no existe.");
            }

            var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == editarRolDTO.RoleName);
            if (role == null)
            {
                errors.Add("El rol no existe.");
            }

            var userHasClaim = false;

            var userClaims = await userManager.GetClaimsAsync(user);
            foreach (var claim in userClaims)
            {
                if (claim.Type == ClaimTypes.Role && claim.Value == editarRolDTO.RoleName)
                {
                    userHasClaim = true;
                }
            }

            if (!userHasClaim)
            {
                errors.Add("El usuario no posee el rol asignado.");
            }

            if (errors.Count > 0)
            {
                return BadRequest(
                    new { errors = errors }
                );
            }
            else
            {
                await userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));
                return Ok("Se removio el rol correctamente.");
            }
        }

        [HttpDelete("{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> EliminarUsuario(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                errors.Add("El usuario no existe.");
                return BadRequest(
                    new { errors = errors }
                );
            }

            await userManager.DeleteAsync(user);
            return Ok("Se elimino el usuario correctamente.");
        }
    }
}
