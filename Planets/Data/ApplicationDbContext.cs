using Microsoft.EntityFrameworkCore;
using Planets.Models;

namespace Planets.Data
{
    /// <summary>
    /// DB context for the application.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Planet> Planets { get; set; }

        public DbSet<PlanetProperty> PlanetProperties { get; set; }

        public DbSet<PlanetPropertyValue> PlanetPropertyValues { get; set; }
    }
}
