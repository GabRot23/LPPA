using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIUAI.DTOs
{
    public class SedeCreacionDTO
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
    }
}