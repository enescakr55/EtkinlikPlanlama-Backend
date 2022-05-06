using Entities;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAccess.Concrete
{
    public class AppDbContext:DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("EventDatabase");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        public DbSet<AccountValidationCode> AccountValidationCodes { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<InvitationStatus> InvitationStatuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<JoinEvent> EventJoins { get; set; }
        public DbSet<UpcomingEvent> UpcomingEvents { get; set; }

    }
}
