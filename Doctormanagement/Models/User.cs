using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Doctormanagement.Models
{
    public partial class User
    {
        public User()
        {
            Doctors = new HashSet<Doctor>();
            Patients = new HashSet<Patient>();
        }
        [Key]
        
        public int User_Id { get; set; }
        
        [MaxLength(30)]
        [Required]
       
        public string? Email { get; set; }
     
        [Required]
        public string? Password { get; set; }
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
           }
}
