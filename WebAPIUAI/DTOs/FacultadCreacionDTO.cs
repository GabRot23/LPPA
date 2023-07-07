using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIUAI.DTOs
{
    public class FacultadCreacionDTO
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
    }
}