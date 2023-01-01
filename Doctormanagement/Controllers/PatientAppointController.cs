using Doctormanagement.Models;
using Doctormanagement.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Doctormanagement.Controllers
{
    public class PatientAppointController : Controller
    {
        private readonly DoctorDbcontext doctorDbcontext;

        public PatientAppointController(DoctorDbcontext doctorDbcontext)
        {
            this.doctorDbcontext = doctorDbcontext;
        }

        [HttpGet]
        public async Task<JsonResult> Index(List<DoctorVM> doctorVM)
        {
           var doctorAppoints = doctorDbcontext.DoctorAppoints.ToList();

           foreach (var item in doctorAppoints)
            {

                int patient_id = doctorDbcontext.PatientAppoints.Where(x => x.Appoint_Id == item.Appoint_Id).FirstOrDefault().Patient_Id;
                int doctor_id = doctorDbcontext.DoctorAppoints.Where(x => x.Appoint_Id == item.Appoint_Id).FirstOrDefault().Doctor_Id;
                int Appointt_id = doctorDbcontext.Appointments.Where(x => x.Appoint_Id == item.Appoint_Id).FirstOrDefault().Appoint_Id;


                var patient_name = doctorDbcontext.Patients.Find(patient_id).Name;
                var patient_age = doctorDbcontext.Patients.Find(patient_id).Age;
                var doctor_name = doctorDbcontext.Doctors.Find(doctor_id).Name;
                var date = doctorDbcontext.Appointments.Find(Appointt_id).Appointed_Date;

                

                DoctorVM doctors = new DoctorVM();

                doctors.Name = patient_name;
                doctors.Age = patient_age;
                doctors.Doctor_Assign =  doctor_name;
                doctors.Date = date;
                
                
                

                doctorVM.Add(doctors);

               
            }
            return Json(doctorVM);
        }
    }
}
