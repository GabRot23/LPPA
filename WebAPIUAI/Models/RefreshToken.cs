using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIUAI.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string Token { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
    }
}