using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAPIUAI.Contracts;

namespace WebAPIUAI.Models
{
    public class Profesor : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int DNI { get; set; }
        public List<MateriasProfesores> MateriasProfesores { get; set; }
    }
}