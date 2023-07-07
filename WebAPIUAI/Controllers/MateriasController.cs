using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIUAI.Data;
using WebAPIUAI.DTOs;
using WebAPIUAI.Models;

namespace WebAPIUAI.Controllers
{
    [ApiController]
    [Route("api/materias")]
    public class MateriasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public MateriasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<MateriaDetallesDTO>>> Get()
        {
            var materias = await context.Materias.ToListAsync();
            var materiasDTO = mapper.Map<List<MateriaDetallesDTO>>(materias);
            return materiasDTO;
        }

        [HttpGet("{id}", Name = "ObtenerMateria")]
        public async Task<ActionResult<MateriaDetallesDTO>> Get(int id)
        {
            var materia = await context.Materias
            .Include(x => x.MateriasProfesores).ThenInclude(x => x.Profesor)
            .Include(x => x.MateriasFacultades).ThenInclude(x => x.Facultad)
            .FirstOrDefaultAsync(x => x.Id == id);
            if (materia == null)
            {
                return NotFound();
            }
            var materiaDTO = mapper.Map<MateriaDetallesDTO>(materia);
            return materiaDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MateriaCreacionDTO materiaCreacionDTO)
        {
            var materia = mapper.Map<Materia>(materiaCreacionDTO);
            context.Add(materia);
            await context.SaveChangesAsync();
            var materiaDTO = mapper.Map<MateriaDetallesDTO>(materia);
            return new CreatedAtRouteResult("ObtenerMateria", new { id = materia.Id }, materiaDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MateriaCreacionDTO materiaCreacionDTO)
        {
            var materia = mapper.Map<Materia>(materiaCreacionDTO);
            materia.Id = id;
            context.Entry(materia).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Materias.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Materia() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}