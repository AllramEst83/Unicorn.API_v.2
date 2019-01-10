using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.API.Entities.Context;

namespace Auth.API.Entities.Seeder
{
    public static class Seeder
    {
        public static void SeedUsers(this AuthContext _context)
        {
            _context.Database.EnsureCreated();

            _context.RemoveRange(_context.Users);
            _context.SaveChangesAsync();

            List<UnicornUser> _users = new List<UnicornUser>
            {
                new UnicornUser
                {
                    FirstName = "Allram",
                    LastName = "Eest",
                    Password = "AssWipe",
                    Token = "",
                    Username ="AllramEst"
                }
            };

            _context.AddRangeAsync(_users);
            _context.SaveChangesAsync();
        }
    }
}
