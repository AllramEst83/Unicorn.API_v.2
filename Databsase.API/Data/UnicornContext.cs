using Databsase.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Databsase.API.Data
{
    public class UnicornContext : DbContext
    {
        public UnicornContext(DbContextOptions<UnicornContext> options) : base(options)
        {

        }

        public DbSet<Unicorn> Unicorns { get; set; }
        public DbSet<HornType> HornTypes { get; set; }

        //dotnet ef migrations add --> namn på migrationen: start
        //dotnet ef database update
    }
}
