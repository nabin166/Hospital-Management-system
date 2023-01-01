using System.ComponentModel.DataAnnotations;

namespace Doctormanagement.Models
{
    public class Appointment
    {
        public Appointment()
        {
            DoctorAppoints = new HashSet<DoctorAppoint>();
            PatientAppoints = new HashSet<PatientAppoint>();
        }
        [Key]
        public int Appoint_Id { get; set; }
        [Required]
        public string Appointed_Date { get; set; }

        public bool IsDeleted { get; set; } = false;
        
        public virtual ICollection<DoctorAppoint> DoctorAppoints { get; set; }
        public virtual ICollection<PatientAppoint> PatientAppoints { get; set; }
    }
}
