using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities
{
    [Table("Movie")]
    public class Movie
    {
        public int Id { get; set; }

        [MaxLength(256)]
        [Required]
        public string Title { get; set; }

        public string? Overview { get; set; }
        [MaxLength(512)]
        public string? Tagline { get; set; }
        public decimal? Budget { get; set; }
        public decimal? Revenue { get; set; }

        [MaxLength(2084)]
        public string? ImdbUrl { get; set; }

        [MaxLength(2084)]
        public string? TmdbUrl { get; set; }

        [MaxLength(2084)]
        public string? PosterUrl { get; set; }

        [MaxLength(2084)]
        public string? BackdropUrl { get; set; }

        [MaxLength(64)]
        public string? OriginalLanguage { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? runTime { get; set; }

        public decimal? Price { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy{ get; set; }
        public string? CreatedBy { get; set; }

        // Navigation Properties
        public ICollection<Trailer> Trailers { get; set; }
        public ICollection<MovieGenre> MoviesOfGenre { get; set; }
        public ICollection<MovieCrew> MoviesOfCrew { get; set; }
        public ICollection<MovieCast> MoviesOfCast { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    }
}
