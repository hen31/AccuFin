using AccuFin.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AccuFin.Data
{
    public class AccuFinDatabaseContext : DbContext
    { 
        public AccuFinDatabaseContext(DbContextOptions<AccuFinDatabaseContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorizedUser>().HasMany<UserAdministrationLink>();
            modelBuilder.Entity<Administration>().HasMany<UserAdministrationLink>();
        }

        public DbSet<AuthorizedUser> AuthorizedUsers { get; set; }
        public DbSet<Administration> Administrations { get; set; }
        public DbSet<UserAdministrationLink> UserAdministrationLink { get; set; }
        public DbSet<BankIntegration> BankIntegrations { get; set; }
    }
}