using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIUAI.Models
{
    public class MateriasFacultades
    {
        public int MateriaId { get; set; }
        public int FacultadId { get; set; }
        public Materia Materia { get; set; }
        public Facultad Facultad { get; set; }
    }
}