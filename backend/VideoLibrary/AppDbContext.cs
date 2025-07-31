using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using VideoLibrary.Models;

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

            modelBuilder.Entity<MovieHasActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId});

            modelBuilder.Entity<GenreHasMovie>()
                .HasKey(gm => new { gm.GenreId, gm.MovieId });

            modelBuilder.Entity<MovieRating>()
                .HasKey(mr => new { mr.MovieId, mr.UserId });

            modelBuilder.Entity<MovieCopy>()
                .HasOne(mc => mc.Movie)
                .WithMany(m=> m.MovieCopies)
                .HasForeignKey(mc => mc.MovieId);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.MovieCopy)
                .WithMany()
                .HasForeignKey(r => r.MovieCopyId);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.BorrowedBy)
                .WithMany()
                .HasForeignKey(r => r.BorrowedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.BorrowedTo)
                .WithMany()
                .HasForeignKey(r => r.BorrowedToId)
                .OnDelete(DeleteBehavior.Restrict);

        }

        public DbSet<Genre> Genres { get; set; }
            public DbSet<Movie> Movies { get; set; }
            public DbSet<Actor> Actors { get; set; }
            public DbSet<Director> Directors { get; set; }
            public DbSet<User> Users { get; set; }
            public DbSet<MovieRating> MovieRatings { get; set; }
            public DbSet<MovieCopy> MovieCopies { get; set; }
            public DbSet<Rental> Rentals { get; set; }
            public DbSet<MovieHasActor> MovieHasActors { get; set; }
            public DbSet<GenreHasMovie> GenreHasMovies { get; set; }


    }


}
