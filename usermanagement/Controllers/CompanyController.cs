using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;  // For async EF methods
using System.Threading.Tasks;
using usermanagement.Data;
using usermanagement.Filters;
using usermanagement.Models;

namespace usermanagement.Controllers
{
    public class CompanyController : Controller
    {
        private readonly Applicationdbcontext applicationdbcontext;

        public CompanyController(Applicationdbcontext applicationdbcontext)
        {
            this.applicationdbcontext = applicationdbcontext;
        }
        
        public async Task<IActionResult> Index()
        {
            var companies = await applicationdbcontext.Companies
                .Where(a => a.isactive == true)
                .ToListAsync();
            return View(companies);
        }
        [AdminOnly]
        public async Task<IActionResult> Edit(int id)
        {
            var company = await applicationdbcontext.Companies.FindAsync(id);
            return View(company);
        }
        [AdminOnly]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await applicationdbcontext.Companies.FindAsync(id);
            if (company != null)
            {
                company.isactive = false;
                applicationdbcontext.Companies.Update(company);
                await applicationdbcontext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        [AdminOnly]
        [HttpPost]
        public async Task<IActionResult> Edit(company company)
        {
            if (ModelState.IsValid)
            {
                applicationdbcontext.Companies.Update(company);
                await applicationdbcontext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(company company)
        {
            
                company company1 = new company()
                {
                    name = company.name,
                    phonenumber = company.phonenumber,
                    description = company.description,
                    email = company.email,
                    isactive = true
                }; 
                await applicationdbcontext.Companies.AddAsync(company1);
                await applicationdbcontext.SaveChangesAsync();
                return RedirectToAction("Index");
            
            
        }
    }
}
