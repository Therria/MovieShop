using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly IMovieRepository _movieRepository;

        public AdminService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<bool> IsMovieExisted(int movieId)
        {
            var movie = await _movieRepository.GetById(movieId);
            if (movie != null)
            {
                return true;
            }

            return false;
        }
    }
}
