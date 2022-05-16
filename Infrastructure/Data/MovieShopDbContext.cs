using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data
{
    public class MovieShopDbContext: DbContext
    {
        public MovieShopDbContext(DbContextOptions<MovieShopDbContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Trailer> Trailers { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<MovieCrew> MovieCrews { get; set; }

        // Fluent API Method

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // specify the rules for Entity
            modelBuilder.Entity<Movie>().Property(m => m.Price).HasPrecision(5,2);
            modelBuilder.Entity<Genre>(builder =>
            {
                builder.ToTable("Genre");
                builder.HasKey(g => g.Id);
                builder.Property(g => g.Name).HasMaxLength(64).IsRequired(); // isrequired = cannot be null
            });
            modelBuilder.Entity<MovieGenre>(ConfigureMovieGenre);
            modelBuilder.Entity<MovieCrew>(ConfigureMovieCrew);
        }

        private void ConfigureMovieGenre(EntityTypeBuilder<MovieGenre> builder)
        {
            builder.ToTable("MovieGenre");
            builder.HasKey(mg => new { mg.MovieId, mg.GenreId });
        }

        private void ConfigureMovieCrew(EntityTypeBuilder<MovieCrew> builder)
        {
            builder.ToTable("MovieCrew");
            builder.HasKey(mc => new { mc.MovieId, mc.CrewId });
            builder.Property(mc => mc.Department).HasMaxLength(128).IsRequired();
            builder.Property(mc => mc.Job).HasMaxLength(128).IsRequired();
        }
    }
}
