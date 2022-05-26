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
    public class GenreService : IGenreService
    {
        private readonly IMovieRepository _movieRepository;

        public GenreService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<List<GenreModel>> GetGenreList()
        {
            var genres = await _movieRepository.GetGenreList();
            var genrelist = new List<GenreModel>();
            foreach (var genre in genres)
            {
                genrelist.Add(new GenreModel
                {
                    Id = genre.Id,
                    Name = genre.Name
                });
            }

            return genrelist;
        }
    }
}
