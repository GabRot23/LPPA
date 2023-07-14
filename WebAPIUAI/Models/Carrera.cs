using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIUAI.Contracts;

namespace WebAPIUAI.Models
{
    public class Carrera : IId
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int FacultadId { get; set; }
        public string Descripcion { get; set; }
        public Facultad Facultad { get; set; }
    }
}