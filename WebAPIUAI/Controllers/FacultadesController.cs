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
    [Route("api/[controller]")]
    public class FacultadesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public FacultadesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<FacultadDTO>>> Get()
        {
            var facultades = await context.Facultades.ToListAsync();
            var facultadesDTO = mapper.Map<List<FacultadDTO>>(facultades);
            return facultadesDTO;
        }

        [HttpGet("{id:int}", Name = "obtenerFacultad")]
        public async Task<ActionResult<FacultadDTO>> Get(int id)
        {
            var facultad = await context.Facultades.FirstOrDefaultAsync(x => x.Id == id);

            if (facultad == null)
            {
                return NotFound();
            }

            var facultadDTO = mapper.Map<FacultadDTO>(facultad);
            return facultadDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FacultadCreacionDTO facultadCreacionDTO)
        {
            var facultad = mapper.Map<Facultad>(facultadCreacionDTO);
            context.Add(facultad);
            await context.SaveChangesAsync();
            var facultadDTO = mapper.Map<FacultadDTO>(facultad);
            return new CreatedAtRouteResult("obtenerFacultad", new { id = facultadDTO.Id }, facultadDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] FacultadCreacionDTO facultadCreacionDTO)
        {
            var facultad = mapper.Map<Facultad>(facultadCreacionDTO);
            facultad.Id = id;
            context.Entry(facultad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Facultades.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Facultad { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}