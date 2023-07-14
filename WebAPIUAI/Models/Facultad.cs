using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAPIUAI.Contracts;

namespace WebAPIUAI.Models
{
    public class Facultad : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        public List<Carrera> Carreras { get; set; }
    }
}