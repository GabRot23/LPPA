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
    public class CarrerasController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CarrerasController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CarreraDTO>>> Get()
        {
            return await Get<Carrera, CarreraDTO>();
        }

        [HttpGet("{id:int}", Name = "obtenerCarrera")]
        public async Task<ActionResult<CarreraConFacultadDTO>> Get(int id)
        {
            var entidad = await context.Carreras.Include(x => x.Facultad).FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<CarreraConFacultadDTO>(entidad);
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CarreraCreacionDTO carreraCreacionDTO)
        {
            return await Post<CarreraCreacionDTO, Carrera, CarreraDTO>(carreraCreacionDTO, "obtenerCarrera");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CarreraCreacionDTO carreraCreacionDTO)
        {
            return await Put<CarreraCreacionDTO, Carrera>(id, carreraCreacionDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Carrera>(id);
        }
    }
}