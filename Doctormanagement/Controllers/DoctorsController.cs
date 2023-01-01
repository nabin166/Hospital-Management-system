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
    public class DoctorsController : Controller
    {
        private readonly DoctorDbcontext _context;

        public DoctorsController(DoctorDbcontext context)
        {
            _context = context;
        }
        //Admin doctor list
        public async Task<IActionResult> Doctorlist()
        {
            var doctorDbcontext = _context.Doctors.Include(d => d.User);
            return View(await doctorDbcontext.ToListAsync());
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            var doctorDbcontext = _context.Doctors.Include(d => d.User).Where(x => x.User.Email == User.Identity.Name);
            return View(await doctorDbcontext.ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Doctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Doctor_Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            ViewData["userid"] = new SelectList(_context.Users, "User_Id", "Email");
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,specialization")] Doctor doctor)
        {
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                int id = _context.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().User_Id;
                doctor.userid = id;

                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["userid"] = new SelectList(_context.Users, "User_Id", "Email", doctor.userid);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Doctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            ViewData["userid"] = new SelectList(_context.Users, "User_Id", "Email", doctor.userid);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Doctor_Id,Name,specialization,userid")] Doctor doctor)
        {
            if (id != doctor.Doctor_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Doctor_Id))
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
            ViewData["userid"] = new SelectList(_context.Users, "User_Id", "Email", doctor.userid);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Doctors == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Doctor_Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Doctors == null)
            {
                return Problem("Entity set 'DoctorDbcontext.Doctors'  is null.");
            }
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
          return _context.Doctors.Any(e => e.Doctor_Id == id);
        }
    }
}
