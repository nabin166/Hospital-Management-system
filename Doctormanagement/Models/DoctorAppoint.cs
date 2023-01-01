using System.ComponentModel.DataAnnotations;

namespace Doctormanagement.Models
{
    public class DoctorAppoint
    {
        [Key]
        public int DocAppId { get; set; }

        public int Doctor_Id { get; set; }
        public int Appoint_Id { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Appointment Appointment { get; set; }
        

    }
}
