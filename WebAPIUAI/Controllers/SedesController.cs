using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPIUAI.Data;
using WebAPIUAI.DTOs;
using WebAPIUAI.Models;

namespace WebAPIUAI.Controllers
{
    [ApiController]
    [Route("api/sedes")]
    public class SedesController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public SedesController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<SedeDTO>>> Get()
        {
            return await Get<Sede, SedeDTO>();
        }

        [HttpGet("{id:int}", Name = "ObtenerSede")]
        public async Task<ActionResult<SedeDTO>> Get(int id)
        {
            return await Get<Sede, SedeDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SedeCreacionDTO sedeCreacionDTO)
        {
            return await Post<SedeCreacionDTO, Sede, SedeDTO>(sedeCreacionDTO, "ObtenerSede");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] SedeCreacionDTO sedeCreacionDTO)
        {
            return await Put<SedeCreacionDTO, Sede>(id, sedeCreacionDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Sede>(id);
        }
    }
}