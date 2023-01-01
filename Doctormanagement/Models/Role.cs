using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Doctormanagement.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }
        [Key]
         
        public int Id { get; set; }
        public string? Roles { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
