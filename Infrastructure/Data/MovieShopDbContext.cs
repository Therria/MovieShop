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

        // Fluent API Method

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // specify the rules for Entity
            modelBuilder.Entity<Genre>(builder =>
            {
                builder.ToTable("Genre");
                builder.HasKey(g => g.Id);
                builder.Property(g => g.Name).HasMaxLength(64).IsRequired(); // isrequired = cannot be null
            });
        }
    }
}
