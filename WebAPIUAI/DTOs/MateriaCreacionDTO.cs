using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPIUAI.Helpers;

namespace WebAPIUAI.DTOs
{
    public class MateriaCreacionDTO
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> FacultadesIds { get; set; }
        public List<int> ProfesoresIds { get; set; }
    }
}