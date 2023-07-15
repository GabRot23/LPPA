using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAPIUAI.Contracts;

namespace WebAPIUAI.Models
{
    public class Carrera : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(200)]
        public string Descripcion { get; set; }
        [Required]
        [Range(1, 100)]
        public int FacultadId { get; set; }
        public Facultad Facultad { get; set; }
    }
}