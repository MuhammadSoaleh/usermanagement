using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using usermanagement.Data;
using usermanagement.Filters;
using usermanagement.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace usermanagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly Applicationdbcontext applicationdbcontext;

        public EmployeeController(Applicationdbcontext applicationdbcontext)
        {
            this.applicationdbcontext = applicationdbcontext;
        }
        
        public IActionResult Index()
        {
            return View(applicationdbcontext.employees.Where(s=>s.isactive==true).ToList());
        }
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var employee = new Employee
            {
                Department = await applicationdbcontext.departments
                    .Where(d => d.isactive)
                    .ToListAsync() ?? new List<Department>(),

                companies = await applicationdbcontext.Companies
                    .Where(c => c.isactive)
                    .ToListAsync() ?? new List<company>()
            };

            return View(employee);
        }
        [AdminOnly]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // DB se employee fetch karo
            var employee = await applicationdbcontext.employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            // Dropdown lists set karo
            employee.Department = await applicationdbcontext.departments
                .Where(d => d.isactive)
                .ToListAsync() ?? new List<Department>();

            employee.companies = await applicationdbcontext.Companies
                .Where(c => c.isactive)
                .ToListAsync() ?? new List<company>();

            // departmentid and companyid set karo based on saved data
            employee.departmentid = applicationdbcontext.departments
                .Where(d => d.departmentname == employee.department)
                .Select(d => d.id)
                .FirstOrDefault();

            employee.companyid = applicationdbcontext.Companies
                .Where(c => c.name == employee.company)
                .Select(c => c.id)
                .FirstOrDefault();

            return View(employee);
        }
        [AdminOnly]
        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            Employee employee2 = new Employee()
            {
                name = employee.name,
                phoneno = employee.phoneno,
                email = employee.email,
                department = employee.department,
                company = employee.company,
                isactive =employee.isactive=true,
            };
            applicationdbcontext.employees.Update(employee);
            applicationdbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

      
        [HttpPost]
        public IActionResult Create(Employee employee) 
        {
        Employee employee2 = new Employee()
        {
            name=employee.name,
            phoneno=employee.phoneno,
            email=employee.email,
            department=employee.department,
            company=employee.company,
            isactive=true,
        };
            applicationdbcontext.employees.Add(employee2);
            applicationdbcontext.SaveChanges();
            return RedirectToAction("Index");
        }
        [AdminOnly]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await applicationdbcontext.employees.FindAsync(id);
            if (company != null)
            {
                company.isactive = false;
                applicationdbcontext.employees.Update(company);
                await applicationdbcontext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
