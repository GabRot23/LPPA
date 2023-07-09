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
    public class ProfesoresController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ProfesoresController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProfesorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            return await Get<Profesor, ProfesorDTO>(paginacionDTO);
        }

        [HttpGet("{id}", Name = "ObtenerProfesor")]
        public async Task<ActionResult<ProfesorDTO>> Get(int id)
        {
            return await Get<Profesor, ProfesorDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProfesorCreacionDTO profesorCreacionDTO)
        {
            return await Post<ProfesorCreacionDTO, Profesor, ProfesorDTO>(profesorCreacionDTO, "ObtenerProfesor");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProfesorCreacionDTO profesorCreacionDTO)
        {
            return await Put<ProfesorCreacionDTO, Profesor>(id, profesorCreacionDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Profesor>(id);
        }
    }
}