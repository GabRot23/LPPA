using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebAPIUAI.DTOs;
using WebAPIUAI.Models;

namespace WebAPIUAI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<IdentityUser, UserDTO>();

            CreateMap<Facultad, FacultadDTO>().ReverseMap();
            CreateMap<FacultadCreacionDTO, Facultad>();
            CreateMap<Facultad, FacultadConCarrerasDTO>().ReverseMap();

            CreateMap<Carrera, CarreraDTO>().ReverseMap();
            CreateMap<CarreraCreacionDTO, Carrera>();
            CreateMap<Carrera, CarreraConFacultadDTO>().ReverseMap();
        }

        // private List<Carrera> MapFacultadCarrerasFacultades(FacultadCreacionDTO facultadCreacionDTO, Facultad facultad)
        // {
        //     var resultado = new List<Carrera>();
        //     if (facultadCreacionDTO.CarrerasIds == null) { return resultado; }

        //     foreach (var id in facultadCreacionDTO.CarrerasIds)
        //     {
        //         resultado.Add(new Carrera() { Id = id });
        //     }

        //     return resultado;
        // }

    }
}