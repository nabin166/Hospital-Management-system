using System.ComponentModel.DataAnnotations;

namespace Doctormanagement.Models
{
    public class PatientAppoint
    {
        [Key]
        public int PatAppid { get; set; }

        public int Patient_Id { get; set; }

        public int Appoint_Id { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual Appointment Appointment { get; set; }


    }
}
