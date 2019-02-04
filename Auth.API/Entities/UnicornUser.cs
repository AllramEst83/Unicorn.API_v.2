using Auth.API.Auth.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.API.Entities
{
    public class UnicornUser
    {
        Roles roleEnum;

        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Roles Role
        {
            get
            {
                return roleEnum;
            }
            set
            {
                roleEnum = value;
            }
        }
    }
}
