using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class ReviewResponseModel
    {
        public int UserId;
        public List<ReviewDetailsModel> ReviewDetails { get; set; }

        public ReviewResponseModel()
        {
            ReviewDetails = new List<ReviewDetailsModel>();
        }
    }
}
