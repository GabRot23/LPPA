using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIUAI.Models;

namespace WebAPIUAI.DTOs
{
    public class MateriaDetallesDTO : MateriaDTO
    {
        public List<FacultadDTO> Facultades { get; set; }
        public List<ProfesorDTO> Profesores { get; set; }
    }
}