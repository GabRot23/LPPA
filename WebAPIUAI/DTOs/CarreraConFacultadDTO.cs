using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIUAI.DTOs
{
    public class CarreraConFacultadDTO : CarreraDTO
    {
        public FacultadDTO Facultad { get; set; }
    }
}