using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIUAI.Models
{
    public class MateriasSedes
    {
        public int MateriaId { get; set; }
        public int SedeId { get; set; }

        public Materia Materia { get; set; }
        public Sede Sede { get; set; }
    }
}