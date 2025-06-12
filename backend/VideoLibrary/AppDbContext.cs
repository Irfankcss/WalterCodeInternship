using Microsoft.EntityFrameworkCore;

namespace VideoLibrary
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // public DbSet<Genre> Genres { get; set; }
        }
    }
}
