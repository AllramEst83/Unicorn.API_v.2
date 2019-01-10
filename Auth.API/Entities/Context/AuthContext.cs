using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.API.Entities.Context
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {

        }

        public DbSet<UnicornUser> Users { get; set; }

        //dotnet ef migrations add --> namn på migrationen: start
        //dotnet ef database update
    }
}
