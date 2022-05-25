using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class PurchaseReponseModel
    {
        public PurchaseReponseModel()
        {
            PurchaseDetails = new List<PurchaseDetailsModel>();
        }

        public int UserId { get; set; }
        
        public List<PurchaseDetailsModel> PurchaseDetails { get; set; }

        
    }
}
