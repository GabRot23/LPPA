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
    public class FacultadesController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public FacultadesController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<FacultadDTO>>> Get()
        {
            return await Get<Facultad, FacultadDTO>();
        }

        [HttpGet("{id:int}", Name = "obtenerFacultad")]
        public async Task<ActionResult<FacultadConCarrerasDTO>> Get(int id)
        {
            var facultad = await context.Facultades
                .Include(x => x.Carreras)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (facultad == null)
            {
                return NotFound("No existe la facultad que desea obtener");
            }

            var facultadDTO = mapper.Map<FacultadConCarrerasDTO>(facultad);
            return facultadDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FacultadCreacionDTO facultadCreacionDTO)
        {
            return await Post<FacultadCreacionDTO, Facultad, FacultadDTO>(facultadCreacionDTO, "obtenerFacultad");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] FacultadCreacionDTO facultadCreacionDTO)
        {
            return await Put<FacultadCreacionDTO, Facultad>(id, facultadCreacionDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Facultad>(id);
        }
    }
}