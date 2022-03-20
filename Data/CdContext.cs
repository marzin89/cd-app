using CdApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CdApp.Data
{
    // Klass som ansluter till databasen
    public class CdContext : DbContext
    {
        // Konstruktor
        public CdContext(DbContextOptions<CdContext> options) : base(options) {}

        // Skapar tabellerna
        public DbSet<Cd> Cd { get; set; }
        public DbSet<Artist> Artist { get; set; }
        public DbSet<User> User { get; set; }
    }
}
