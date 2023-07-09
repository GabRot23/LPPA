using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIUAI.DTOs
{
    public class ResultadoHash
    {
        public string Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}