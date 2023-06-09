using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAPIUAI.Contracts;

namespace WebAPIUAI.Models
{
    public class Materia : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public List<MateriasFacultades> MateriasFacultades { get; set; }
        public List<MateriasProfesores> MateriasProfesores { get; set; }
        public List<MateriasSedes> MateriasSedes { get; set; }
    }
}