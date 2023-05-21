using AccuFin.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccuFin.Data
{
    public class AccuFinDatabaseContext : DbContext
    {
        public AccuFinDatabaseContext(DbContextOptions<AccuFinDatabaseContext> options) : base(options)
        {
        }

        public DbSet<AuthorizedUser> AuthorizedUsers { get; set; }
        public DbSet<Administration> Administrations { get; set; }
        public DbSet<UserAdministrationLink> UserAdministrationLink { get; set; }
    }
}