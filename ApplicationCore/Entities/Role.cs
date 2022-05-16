using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    [Table("Role")]
    public class Role
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string? Name { get; set; }

        // Navigation Properties
        public ICollection<UserRole> RolesOfUser { get; set; }
    }
}
