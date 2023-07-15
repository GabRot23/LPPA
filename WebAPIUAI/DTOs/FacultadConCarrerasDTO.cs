using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIUAI.DTOs
{
    public class FacultadConCarrerasDTO : FacultadDTO
    {
        public List<CarreraDTO> Carreras { get; set; }
    }
}