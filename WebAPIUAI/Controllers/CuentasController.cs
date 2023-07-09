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
        }

        [HttpPost("Crear")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return await ConstruirToken(model);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("Login")]
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
                return BadRequest("Login incorrecto");
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
                FechaCreacion = DateTime.UtcNow,
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

            var expiration = DateTime.UtcNow.AddYears(1);

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
        public async Task<ActionResult<List<UserClaimsDTO>>> GetUserClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("El usuario no existe");
            }

            var claims = await userManager.GetClaimsAsync(user);

            return claims.Select(x => new UserClaimsDTO { ClaimType = x.Type, ClaimValue = x.Value }).ToList();
        }


        //Post para crear roles
        [HttpPost("CrearRol")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> CrearRol([FromBody] CrearRolDTO crearRolDTO)
        {
            var role = new IdentityRole { Name = crearRolDTO.Nombre, NormalizedName = crearRolDTO.Nombre.ToUpper() };
            await context.Roles.AddAsync(role);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("AsignarRol")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> AsignarRol(EditarUserRolDTO editarRolDTO)
        {
            var user = await userManager.FindByIdAsync(editarRolDTO.UserId);
            if (user == null)
            {
                return NotFound("El usuario no existe");
            }

            //Validar si el rol existe
            var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == editarRolDTO.RoleName);
            if (role == null)
            {
                return NotFound("El rol no existe");
            }

            //Validar si el usuario ya tiene el rol
            var userHasRole = await userManager.IsInRoleAsync(user, editarRolDTO.RoleName);
            if (userHasRole)
            {
                return BadRequest("El usuario ya tiene el rol");
            }

            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));
            return NoContent();
        }

        [HttpPost("RemoverRol")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> RemoverRol(EditarUserRolDTO editarRolDTO)
        {
            var user = await userManager.FindByIdAsync(editarRolDTO.UserId);
            if (user == null)
            {
                return NotFound("El usuario no existe");
            }

            var role = await context.Roles.FirstOrDefaultAsync(x => x.Name == editarRolDTO.RoleName);
            if (role == null)
            {
                return NotFound("El rol no existe");
            }

            var userHasRole = await userManager.IsInRoleAsync(user, editarRolDTO.RoleName);
            if (!userHasRole)
            {
                return BadRequest("El usuario no tiene el rol");
            }

            await userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));
            return NoContent();
        }
    }
}
