using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace TaxiApp.Models
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<DriverRequest> Drivers { get; set; }

        public DbSet<Drive> Drives { get; set; } 

        public DbSet<UserDrive> UserDrives { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
