using Doctormanagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace Doctormanagement.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly DoctorDbcontext _context;
        public AppointmentController(DoctorDbcontext doctorDbcontext)
        {
            _context =  doctorDbcontext;

        }
        [HttpPost]
        public async Task<IActionResult> Add(int pid , string date , string did)
        {
           Appointment appointment = new Appointment();
           appointment.Appointed_Date = date;
           await _context.AddAsync(appointment);
           await _context.SaveChangesAsync();


            var Appointed_id = _context.Appointments.Where(x => x.Appointed_Date == date).First().Appoint_Id;
            var doctor_id = _context.Doctors.Where(x=>x.Name == did).First().Doctor_Id;
           
            PatientAppoint patientAppoint = new PatientAppoint();
            patientAppoint.Appoint_Id = Appointed_id;
            patientAppoint.Patient_Id = pid;
            await _context.AddAsync(patientAppoint);
            await _context.SaveChangesAsync();

            DoctorAppoint doctorAppoint = new DoctorAppoint();
            doctorAppoint.Appoint_Id = Appointed_id;
            doctorAppoint.Doctor_Id = doctor_id;
            await _context.AddAsync(doctorAppoint);
            await _context.SaveChangesAsync();









            return RedirectToAction("Assign","Patients");
        }
    }
}
