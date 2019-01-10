using Auth.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.API
{
    public interface IAuthServices
    {
        UnicornUser Authenticate(string username, string password);
        IEnumerable<object> GetAll();
        void DecryptToken(string token);
    }
}
