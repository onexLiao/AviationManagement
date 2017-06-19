using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviationManagement.Models
{
    public static class DbInitializer
    {
        public static void Initialize(WebAPIDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (!context.Areas.Any())
            {
                var areas = new List<Area>()
                {
                    new Area() { Name = "Guangzhou", AreaID = "gz", },
                    new Area() { Name = "Shanghai", AreaID = "sh", },
                    new Area() { Name = "Beijing", AreaID = "bj", },
                    new Area() { Name = "Xian", AreaID = "xa", },
                    new Area() { Name = "Wuhan", AreaID = "wh", },
                    new Area() { Name = "Sanya", AreaID = "sy", }
                };
                areas.ForEach(a => context.Areas.Add(a));
                context.SaveChanges();
            }// DB has been seeded

            // Add Planes
            if (!context.Planes.Any())
            {
                var planes = new List<Plane>()
                {
                    new Plane()
                    {
                        PlaneID = "AirBus 330", VipRows = 3, BussinessRows = 7, EconomyRows = 35,
                        VipColumnName = "ABEFGH", BussinessColumnName = "ABEFGH", EconomyColumnName = "ABCDEFGH",
                        VipSeats = 18, BussinessSeats = 42, EconomySeats = 280
                    },
                    new Plane()
                    {
                        PlaneID = "Boeing 747－400", VipRows = 2, BussinessRows = 4, EconomyRows = 30,
                        VipColumnName = "ABEFJK", BussinessColumnName = "ABDEFJK", EconomyColumnName = "ABCDEFGHJK",
                        VipSeats = 12, BussinessSeats = 28, EconomySeats = 300
                    }
                };
                planes.ForEach(p => context.Planes.Add(p));
                context.SaveChanges();
            }
            
        }
    }
}
