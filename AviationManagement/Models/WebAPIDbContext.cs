using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AviationManagement.Models
{
    public class WebAPIDbContext : DbContext
    {
        public WebAPIDbContext(DbContextOptions<WebAPIDbContext> options)
            : base(options)
        {
        }

        public DbSet<Area> Areas { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerAlthorithm> CustomerAlthorithms { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Plane> Planes { get; set; }

        public DbSet<Seat> Seats { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Sets the properties that make up the primary key for this entity type.
            builder.Entity<Area>().HasKey(m => m.AreaID);

            builder.Entity<Customer>().HasKey(c => c.CustomerID);
            //builder.Entity<Customer>()
            //    .HasOne(c => c.CustomerAlthorithm)
            //    .WithOne(c => c.Customer);

            builder.Entity<CustomerAlthorithm>().HasKey(c => c.CustomerAlthorithmID);

            builder.Entity<Flight>().HasKey(f => f.FligtID);

            builder.Entity<Plane>().HasKey(p => p.PlaneID);

            builder.Entity<Seat>().HasKey(s => new { s.FlightID, s.SeatNum });

            base.OnModelCreating(builder);
        }
    }
}
