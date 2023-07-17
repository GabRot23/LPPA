using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebAPIUAI.Data;
using WebAPIUAI.DTOs;
using WebAPIUAI.Models;

namespace WebAPIUAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarrerasController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private List<string> errors;

        public CarrerasController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            errors = new List<string>();
        }

        [HttpGet]
        public async Task<ActionResult<List<CarreraDTO>>> Get()
        {
            return await Get<Carrera, CarreraDTO>();
        }

        [HttpGet("{id:int}", Name = "obtenerCarrera")]
        public async Task<ActionResult<CarreraConFacultadDTO>> Get(int id)
        {
            var carrera = await context.Carreras
                .Include(x => x.Facultad)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (carrera == null)
            {
                return NotFound("No existe la carrera que desea obtener");
            }

            var carreraDTO = mapper.Map<CarreraConFacultadDTO>(carrera);
            return carreraDTO;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Post([FromBody] CarreraCreacionDTO carreraCreacionDTO)
        {
            // Se verifica que el nombre de la carrera no exista en otra carrera
            if (await ExisteCarreraNombre(carreraCreacionDTO.Nombre))
                errors.Add($"Ya existe una carrera con el nombre {carreraCreacionDTO.Nombre}");

            // Se verifica que exista la facultad a la que se quiere asignar la carrera
            if (!await ExisteFacultadId(carreraCreacionDTO.FacultadId))
                errors.Add($"No existe una facultad con el id {carreraCreacionDTO.FacultadId}");

            // Si hay errores se devuelven
            if (errors.Count > 0)
            {
                return UnprocessableEntity(new { errors = errors });
            }

            return await Post<CarreraCreacionDTO, Carrera, CarreraDTO>(carreraCreacionDTO, "obtenerCarrera");
        }

        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Put(int id, [FromBody] CarreraCreacionDTO carreraCreacionDTO)
        {
            // Se verifica que exista la carrera que se quiere modificar
            if (!await ExisteCarreraId(id))
            {
                return NotFound("No existe la carrera que desea modificar");
            }

            // // Se verifica que el nombre de la carrera no exista en otra carrera
            // if (await ExisteCarreraNombre(carreraCreacionDTO.Nombre))
            // {
            //     return BadRequest($"Ya existe una carrera con el nombre {carreraCreacionDTO.Nombre}");
            // }

            // Se verifica que exista la facultad a la que se quiere asignar la carrera
            if (!await ExisteFacultadId(carreraCreacionDTO.FacultadId))
            {
                return BadRequest($"No existe una facultad con el id {carreraCreacionDTO.FacultadId}");
            }

            return await Put<CarreraCreacionDTO, Carrera>(id, carreraCreacionDTO);
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            // Se verifica que exista la carrera que se quiere eliminar
            if (!await ExisteCarreraId(id))
            {
                return NotFound("No existe la carrera que desea eliminar");
            }

            return await Delete<Carrera>(id);
        }

        private async Task<bool> ExisteCarreraNombre(string nombre)
        {
            var carrera = await context.Carreras.AsNoTracking().FirstOrDefaultAsync(x => x.Nombre == nombre);
            if (carrera == null)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> ExisteCarreraId(int id)
        {
            var carrera = await context.Carreras.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (carrera == null)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> ExisteFacultadId(int id)
        {
            var facultad = await context.Facultades.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (facultad == null)
            {
                return false;
            }
            return true;
        }
    }
}