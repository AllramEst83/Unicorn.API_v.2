using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.API.Auth.API.Models
{
    public class UserClaimsModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Role { get; set; }
        public string TokenExpiration { get; set; }
    }
}
