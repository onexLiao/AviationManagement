using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Threading;

namespace AviationManagement.Models.Manager
{
    public static class DbInitializer
    {
        public static void Initialize(WebAPIDbContext context)
        {
            bool success = false;
            // 如果连接失败则等一段时间后重试
            while (!success)
            {
                try
                {
                    InitDB(context);
                    success = true;
                }
                catch (MySqlException)
                {
                    Thread.Sleep(3000);
                }
            }
        }

        public static void InitDB(WebAPIDbContext context)
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
                    new Plane() { VipSeats = 18, BussinessSeats = 42, EconomySeats = 280 },
                    new Plane() { VipSeats = 12, BussinessSeats = 28, EconomySeats = 300 }
                };
                planes.ForEach(p => context.Planes.Add(p));
                context.SaveChanges();
            }

            if (!context.CustomerAlthorithms.Any())
            {
                var althorithms = new List<CustomerAlthorithm>()
                {
                    new CustomerAlthorithm() { ID = Guid.NewGuid(), Account = "leetcode", Password = "leetcode" },
                    new CustomerAlthorithm() { ID = Guid.NewGuid(), Account = "xian", Password = "xian" }
                };
                var withprofile = new CustomerAlthorithm() { ID = Guid.NewGuid(), Account = "xua", Password = "xua" };
                var profile = new CustomerProfile()
                {
                    CustomerAlthorithm = withprofile,
                    CustomerAlthorithmID = withprofile.ID,
                    CustomerProfileID = Guid.NewGuid(),
                    EMail = "zjxuan1996@outlook.com",
                    Sex = Sex.Male
                };
                althorithms.Add(withprofile);
                althorithms.ForEach(c => context.CustomerAlthorithms.Add(c));
                context.CustomerProfiles.Add(profile);
                context.SaveChanges();
            }


        }
    }
}
