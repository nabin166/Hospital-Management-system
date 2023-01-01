using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Doctormanagement.Models;
using Microsoft.AspNetCore.Authorization;

using BCryptNet = BCrypt.Net.BCrypt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Doctormanagement.Controllers
{
    
    public class UsersController : Controller
    {
        private readonly DoctorDbcontext _context;

        public UsersController(DoctorDbcontext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Login()
        {
            
            return View();
        }

      
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Signin(string Email, string Password)
            {
                User user = _context.Users.Where(x => x.Email == Email).FirstOrDefault();


                ClaimsIdentity identity = null!;
                bool isAuthenticate = false;

                if (user != null)
                {
                 
                        if (BCrypt.Net.BCrypt.Verify(Password, user.Password))
                        {
                            string roless = _context.Roles.Where(x => x.Users.Where(x => x.Email == Email).Count() > 0).FirstOrDefault().Roles;
                            identity = new ClaimsIdentity(new[]
                            {
                        new Claim(ClaimTypes.Name, Email),
                        new Claim(ClaimTypes.Role, roless),

                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                            isAuthenticate = true;

                            if (isAuthenticate)
                            {
                                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home", new { issuccess = true });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { issuccess = true });
                    }
                
               



                return RedirectToAction("Index", "Login");
            }


            public IActionResult Logout()
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Users");
            }
        

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var doctorDbcontext = _context.Users.Include(u => u.Role);
            return View(await doctorDbcontext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.User_Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(x=>x.Roles != "Admin"), "Id", "Roles");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("User_Id,Email,Password,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Where(x => x.Email == user.Email).Count() != 1)
                {


                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    user.Password = passwordHash;
                   


                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login","Users");
                }
                else
                {
                    ViewData["error"] = "Username already exist";
                    return RedirectToAction("Register", "Account");
                }


            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("User_Id,Email,Password,RoleId")] User user)
        {
            if (id != user.User_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.User_Id))
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.User_Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'DoctorDbcontext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return _context.Users.Any(e => e.User_Id == id);
        }
    }
}
