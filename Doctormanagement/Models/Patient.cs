using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Doctormanagement.Models
{
    [Authorize]
    public class Patient
    {
        public Patient()
        {
            PatientAppoints = new HashSet<PatientAppoint>();
            
        }
        [Key]
        public int Patient_Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age {get; set; }
        
        public DateTime CreatedDate { get; set; }

        [Required]
        public string Category { get; set; }
        public int userid { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<PatientAppoint> PatientAppoints { get; set; }

    }
}
