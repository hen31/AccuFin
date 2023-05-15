using AccuFin.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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