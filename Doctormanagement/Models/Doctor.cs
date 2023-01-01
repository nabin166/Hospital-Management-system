using System.ComponentModel.DataAnnotations;

namespace Doctormanagement.Models
{
    public class Doctor
    {
        public Doctor()
        {
            DoctorAppoints = new HashSet<DoctorAppoint>();
        }
        [Key]
        public int Doctor_Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? specialization { get; set; }
        public int userid { get; set; }

        public virtual string CheckName { get => Name + "(" + specialization + ")"; }

        public virtual User User { get; set; }
        
        public virtual ICollection<DoctorAppoint> DoctorAppoints { get; set; }
    }
}
