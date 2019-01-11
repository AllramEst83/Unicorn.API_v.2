using Auth.API.Auth.API.Models;
using Auth.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.API
{
    public interface IAuthServices
    {
        IEnumerable<object> GetAll();
        AuthAPIUser Auth(UnicornUser _user);
        UserClaimsModel DecodeToken(string tokenString);
        bool UserExistsWithRole(string mustHaveRole, UserClaimsModel requestUser);
    }
}
