using Databsase.API.Data;
using Databsase.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Databsase.API.DataAccess.Seeder
{
    public static class Seeder
    {

        public static void SeedUnicorns(this UnicornContext context)
        {
            context.Database.EnsureCreated();

            context.RemoveRange(context.Unicorns);
            context.RemoveRange(context.HornTypes);
            context.SaveChanges();

            Guid hornOne = Guid.NewGuid();
            Guid hornTwo = Guid.NewGuid();

            List<HornType> horntypes = new List<HornType>()
            {
                new HornType()
                {
                    Id = hornOne,
                    Name ="Silver spire"
                },
                new HornType()
                {
                    Id = hornTwo,
                    Name ="Emerald blis"
                }
            };

            context.AddRangeAsync(horntypes);
            context.SaveChangesAsync();

            List<Unicorn> unicorns = new List<Unicorn>()
            {
                new Unicorn()
                {
                   Name = "Ju-ju-Bee",
                   HornType = new HornType(){ Id = hornOne, Name ="Silver spire"},
                   DateOfBirth = new DateTime(2000,12,12)
                },
                new Unicorn()
                {
                    Name = "Bucking Bronco",
                    HornType =  new HornType(){Id = hornTwo,Name ="Emerald blis"},
                    DateOfBirth = new DateTime(1515,09,04)
                },
            };

            context.Unicorns.AddRangeAsync(unicorns);
            context.SaveChangesAsync();
        }

    }
}
