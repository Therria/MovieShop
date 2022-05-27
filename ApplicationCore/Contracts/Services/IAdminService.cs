using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Services
{
    public interface IAdminService
    {
        Task<bool> CreateMovie(MovieCreateRequestModel request, string userName);
        Task<bool> UpdateMovie(MovieUpdateRequestModel request, string userName);
        Task<List<MovieCardModel>> GetTopPurchasedMovie(DateTime fromDate, DateTime toDate);
    }
}
