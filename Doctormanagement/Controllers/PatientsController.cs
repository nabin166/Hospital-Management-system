using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Doctormanagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace Doctormanagement.Controllers
{
    [Authorize]
    public class PatientsController : Controller
    {
        private readonly DoctorDbcontext _context;

        public PatientsController(DoctorDbcontext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Assign()
        {
           //Where(x => x.PatientAppoints.Where(x => x.Patient_Id != x.Patient.Patient_Id).Count() >= 1)
            var doctorDbcontext = _context.Patients.Include(p => p.User).Include(p => p.PatientAppoints).Where(x => x.PatientAppoints.Where(x => x.Patient_Id == x.Patient.Patient_Id).Count() != 1); ;
            ViewData["Doctor"] = new SelectList(_context.Doctors, "Doctor_Id", "CheckName");
            return View(await doctorDbcontext.ToListAsync());
        }

        public async Task<IActionResult> Assigned()
        {
            
            var doctorDbcontext = _context.Patients.Include(p => p.User).Include(p=>p.PatientAppoints).Where(x=>x.PatientAppoints.Where(x=>x.Patient_Id == x.Patient.Patient_Id).Count() == 1);
            ViewData["Doctor"] = new SelectList(_context.Doctors, "Doctor_Id", "CheckName");
            return View(await doctorDbcontext.ToListAsync());
        }


        // GET: Patients
        public async Task<IActionResult> Index()
        {
            var doctorDbcontext = _context.Patients.Include(p => p.User).Where(x => x.User.Email == User.Identity.Name);
            return View(await doctorDbcontext.ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Patient_Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            ViewData["userid"] = new SelectList(_context.Users, "User_Id", "Email");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Age,CreatedDate,Category")] Patient patient)
        {
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                int id = _context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().User_Id;
                patient.userid = id;

                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["userid"] = new SelectList(_context.Users, "User_Id", "Email", patient.userid);
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["userid"] = new SelectList(_context.Users, "User_Id", "Email", patient.userid);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Patient_Id,Name,Age,CreatedDate,Category,userid")] Patient patient)
        {
            if (id != patient.Patient_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Patient_Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["userid"] = new SelectList(_context.Users, "User_Id", "Email", patient.userid);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Patient_Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Patients == null)
            {
                return Problem("Entity set 'DoctorDbcontext.Patients'  is null.");
            }
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
          return _context.Patients.Any(e => e.Patient_Id == id);
        }
    }
}
