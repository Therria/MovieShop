using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class UserDetailsModel
    {
        public UserDetailsModel()
        {
            Reviews = new List<ReviewDetailsModel>();
            Purchases = new List<PurchaseDetailsModel>();
            Favorites = new List<FavoriteDetailsModel>();
            Roles = new List<RoleModel>();
        }

        public int Id { get; set; }

        public string? LastName { get; set; }

        public string? FirstName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string? Email { get; set; }
        public string? HashedPassword { get; set; }
        public string? Salt { get; set; }
        public string? PhoneNumber { get; set; }
        //public bool? TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDate { get; set; }
        public DateTime? LastLoginDateTime { get; set; }
        //public bool? IsLocked { get; set; }
        //public int? AccessFailedCount { get; set; }

        public List<ReviewDetailsModel> Reviews { get; set; }
        public List<PurchaseDetailsModel> Purchases { get; set; }
        public List<FavoriteDetailsModel> Favorites { get; set; }
        public List<RoleModel> Roles { get; set; }
    }
}
