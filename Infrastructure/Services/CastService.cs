using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CastService : ICastService
    {
        private readonly ICastRepository _castRepository;

        public CastService(ICastRepository castRepository)
        {
            _castRepository = castRepository;
        }

        public async Task<CastDetailsModel> GetCastDetails(int castId)
        {
            var casts = await _castRepository.GetById(castId);
            var castDetails = new CastDetailsModel
            {
                Id = casts.Id,
                Name = casts.Name,
                Gender = casts.Gender,
                TmdbUrl = casts.TmdbUrl,
                ProfilePath = casts.ProfilePath
            };

            foreach (var movie in casts.CastsOfMovie)
            {
                castDetails.Movies.Add(new MovieCardModel { Id = movie.MovieId, PosterUrl = movie.Movie.PosterUrl, 
                    Title = movie.Movie.Title });
            }

            return castDetails;
        }
    }
}
