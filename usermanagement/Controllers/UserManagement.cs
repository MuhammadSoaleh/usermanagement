using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using usermanagement.Data;
using usermanagement.Models;
using usermanagement.Filters;
namespace usermanagement.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly Applicationdbcontext _context;

        public UserManagementController(Applicationdbcontext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var admins = await _context.SuperAdmins
                .Where(s => s.isactive)
                .ToListAsync();
            return View(admins);
        }
        [AdminOnly]
        public async Task<IActionResult> Edit(int id)
        {
            var admin = await _context.SuperAdmins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }
        [AdminOnly]
        [HttpPost]
        public async Task<IActionResult> Edit(SuperAdmin superAdmin)
        {
            if (ModelState.IsValid)
            {
                var existingAdmin = await _context.SuperAdmins.FindAsync(superAdmin.id);
                if (existingAdmin == null)
                {
                    return NotFound();
                }

                existingAdmin.username = superAdmin.username;
                existingAdmin.password = superAdmin.password;

                _context.SuperAdmins.Update(existingAdmin);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(superAdmin);
        }
        [AdminOnly]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var admin = await _context.SuperAdmins.FindAsync(id);
                if (admin == null)
                {
                    return NotFound();
                }

                admin.isactive = false;

                _context.SuperAdmins.Update(admin);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong while deleting.");
            }
        }

        // GET: /UserManagement/Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(SuperAdmin superAdmin)
        {
            if (ModelState.IsValid)
            {
                var admin = await _context.SuperAdmins
                    .FirstOrDefaultAsync(a => a.username == superAdmin.username && a.password == superAdmin.password && a.isactive);

                if (admin != null)
                {
                    // ✅ Create claims
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.username),
                new Claim("IsAdmin", "true") // ⚠️ or use admin.isadmin.ToString().ToLower() if it's a property
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // ✅ Sign in user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "UserManagement");
                }

                ModelState.AddModelError("", "Invalid username or password");
            }

            return View(superAdmin);
        }

        //// POST: /UserManagement/Login
        //[HttpPost]
        //public async Task<IActionResult> Login(SuperAdmin superAdmin)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var admin = await _context.SuperAdmins
        //            .FirstOrDefaultAsync(a => a.username == superAdmin.username && a.password == superAdmin.password);

        //        if (admin != null)
        //        {
        //            HttpContext.Session.SetString("Username", admin.username);
        //            HttpContext.Session.SetInt32("AdminId", admin.id);

        //            return RedirectToAction("Index");
        //        }

        //        ModelState.AddModelError("", "Invalid username or password");
        //    }

        //    return View(superAdmin);
        //}
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Register(SuperAdmin superAdmin)
        {
            SuperAdmin superAdmin1 = new SuperAdmin()
            { 
                username=superAdmin.username,
                password=superAdmin.password,
                isactive=true
            }; 
            await _context.SuperAdmins.AddAsync(superAdmin1);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login", "UserManagement");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "UserManagement");
        }
        public IActionResult AccessDenied() 
        {
            return View();
        }

    }
}
