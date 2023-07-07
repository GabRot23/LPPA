using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIUAI.Data;
using WebAPIUAI.DTOs;
using WebAPIUAI.Helpers;
using WebAPIUAI.Models;

namespace WebAPIUAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfesoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ProfesoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProfesorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Profesores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);

            var profesores = await queryable.Paginar(paginacionDTO).ToListAsync();
            var profesoresDTO = mapper.Map<List<ProfesorDTO>>(profesores);
            return profesoresDTO;
        }

        [HttpGet("{id}", Name = "ObtenerProfesor")]
        public async Task<ActionResult<ProfesorDTO>> Get(int id)
        {
            var profesor = await context.Profesores.FirstOrDefaultAsync(x => x.Id == id);
            if (profesor == null)
            {
                return NotFound();
            }
            var profesorDTO = mapper.Map<ProfesorDTO>(profesor);
            return profesorDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProfesorCreacionDTO profesorCreacionDTO)
        {
            var profesor = mapper.Map<Profesor>(profesorCreacionDTO);
            context.Add(profesor);
            await context.SaveChangesAsync();
            var profesorDTO = mapper.Map<ProfesorDTO>(profesor);
            return new CreatedAtRouteResult("ObtenerProfesor", new { id = profesor.Id }, profesorDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProfesorCreacionDTO profesorCreacionDTO)
        {
            var profesor = mapper.Map<Profesor>(profesorCreacionDTO);
            profesor.Id = id;
            context.Entry(profesor).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Profesores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Profesor { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}