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
            CreateMap<Facultad, FacultadDTO>().ReverseMap();
            CreateMap<FacultadCreacionDTO, Facultad>();

            CreateMap<IdentityUser, UserDTO>();
            CreateMap<Sede, SedeDTO>().ReverseMap();
            CreateMap<SedeCreacionDTO, Sede>();
            CreateMap<Profesor, ProfesorDTO>().ReverseMap();
            CreateMap<ProfesorCreacionDTO, Profesor>();
            CreateMap<Materia, MateriaDetallesDTO>().ReverseMap();
            CreateMap<MateriaCreacionDTO, Materia>()
            .ForMember(x => x.MateriasFacultades, options => options.MapFrom(MapMateriaFacultades))
            .ForMember(x => x.MateriasProfesores, options => options.MapFrom(MapMateriaProfesores));

            CreateMap<Materia, MateriaDetallesDTO>()
            .ForMember(x => x.Facultades, options => options.MapFrom(MapMateriaFacultades))
            .ForMember(x => x.Profesores, options => options.MapFrom(MapMateriaProfesores));
        }

        private List<MateriasFacultades> MapMateriaFacultades(MateriaCreacionDTO materiaCreacionDTO, Materia materia)
        {
            var resultado = new List<MateriasFacultades>();
            if (materiaCreacionDTO.FacultadesIds == null) { return resultado; }
            foreach (var facultadId in materiaCreacionDTO.FacultadesIds)
            {
                resultado.Add(new MateriasFacultades() { FacultadId = facultadId });
            }
            return resultado;
        }

        private List<MateriasProfesores> MapMateriaProfesores(MateriaCreacionDTO materiaCreacionDTO, Materia materia)
        {
            var resultado = new List<MateriasProfesores>();
            if (materiaCreacionDTO.ProfesoresIds == null) { return resultado; }
            foreach (var profesorId in materiaCreacionDTO.ProfesoresIds)
            {
                resultado.Add(new MateriasProfesores() { ProfesorId = profesorId });
            }
            return resultado;
        }

        private List<FacultadDTO> MapMateriaFacultades(Materia materia, MateriaDetallesDTO materiaDetallesDTO)
        {
            var resultado = new List<FacultadDTO>();
            if (materia.MateriasFacultades != null)
            {
                foreach (var facultad in materia.MateriasFacultades)
                {
                    resultado.Add(new FacultadDTO() { Id = facultad.FacultadId, Nombre = facultad.Facultad.Nombre });
                }
            }
            return resultado;
        }

        private List<ProfesorDTO> MapMateriaProfesores(Materia materia, MateriaDetallesDTO materiaDetallesDTO)
        {
            var resultado = new List<ProfesorDTO>();
            if (materia.MateriasProfesores != null)
            {
                foreach (var profesor in materia.MateriasProfesores)
                {
                    resultado.Add(new ProfesorDTO() { Id = profesor.ProfesorId, Nombre = profesor.Profesor.Nombre });
                }
            }
            return resultado;
        }
    }
}