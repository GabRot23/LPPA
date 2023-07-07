using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIUAI.Models
{
    public class MateriasProfesores
    {
        public int MateriaId { get; set; }
        public int ProfesorId { get; set; }
        public Materia Materia { get; set; }
        public Profesor Profesor { get; set; }
    }
}