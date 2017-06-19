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
            if (context.Areas.Any())
            {
                return;   // DB has been seeded
            }

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


        }
    }
}
