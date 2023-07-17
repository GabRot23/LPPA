using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIUAI.DTOs;

namespace WebAPIUAI.DTOs
{
    public class UserClaimsDTO : UserDTO
    {
        public List<ClaimDTO> Claims { get; set; }
    }
}