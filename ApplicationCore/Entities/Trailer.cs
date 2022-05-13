using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Trailer
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string? TitleUrl { get; set; }
        public string? Name { get; set; }


    }
}
