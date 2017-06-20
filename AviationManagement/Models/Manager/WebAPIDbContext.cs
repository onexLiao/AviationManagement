using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AviationManagement.Models;

namespace AviationManagement.Models.Manager
{
    public class WebAPIDbContext : DbContext
    {
        public WebAPIDbContext(DbContextOptions<WebAPIDbContext> options)
            : base(options)
        {
        }

        public DbSet<Area> Areas { get; set; }

        public DbSet<CustomerProfile> CustomerProfiles { get; set; }

        public DbSet<CustomerAlthorithm> CustomerAlthorithms { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Plane> Planes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Sets the properties that make up the primary key for this entity type.
            builder.Entity<Area>().HasKey(m => m.AreaID);

            builder.Entity<CustomerAlthorithm>().HasKey(c => c.ID);
            builder.Entity<CustomerAlthorithm>().Property(c => c.Account).IsRequired();
            builder.Entity<CustomerAlthorithm>().Property(c => c.Password).IsRequired();

            builder.Entity<CustomerAlthorithm>().HasAlternateKey(c => c.Account);
            builder.Entity<CustomerAlthorithm>()
                .HasOne(c => c.CustomerProfile)
                .WithOne(c => c.CustomerAlthorithm)
                .HasForeignKey<CustomerProfile>(c => c.CustomerAlthorithmID);

            builder.Entity<CustomerProfile>().HasKey(c => c.CustomerProfileID);
            builder.Entity<CustomerProfile>().Property(c => c.CustomerAlthorithmID).IsRequired();
            builder.Entity<CustomerProfile>()
                .HasMany(c => c.Tickets)
                .WithOne(t => t.Customer);


            builder.Entity<Plane>().HasKey(p => p.PlaneID);
            builder.Entity<Plane>().Ignore(p => p.TotalSeats);

            builder.Entity<Flight>().HasKey(f => f.FligtID);
            builder.Entity<Flight>()
                .HasMany(f => f.Tickets)
                .WithOne(t => t.Flight);

            builder.Entity<Ticket>().HasKey(t => t.TicketID);

            base.OnModelCreating(builder);
        }

        public DbSet<AviationManagement.Models.Ticket> Ticket { get; set; }
    }
}
