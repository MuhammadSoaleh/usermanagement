using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using usermanagement.Data;
using usermanagement.Filters;
using usermanagement.Models;

namespace usermanagement.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly Applicationdbcontext applicationdbcontext;

        public DepartmentController(Applicationdbcontext applicationdbcontext)
        {
            this.applicationdbcontext = applicationdbcontext;
        }
        
        public async Task<IActionResult> Index()
        {
            var departments = await applicationdbcontext.departments
                .Where(s => s.isactive)
                .ToListAsync();
            return View(departments);
        }
     
        public IActionResult Create()
        {
            var model = new Department();
            model.Companies = applicationdbcontext.Companies.Where(c => c.isactive).ToList();  // Fill the list of companies
            return View(model);
        }

       
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
           
                Department department1 = new Department()
                {
                    companyid = department.companyid,
                    companyname = department.companyname,
                    departmentname = department.departmentname,
                      
                };
                await applicationdbcontext.departments.AddAsync(department1);
                await applicationdbcontext.SaveChangesAsync();
                return RedirectToAction("Index");
          
            
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await applicationdbcontext.departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var model = new Department
            {
                id = department.id,
                departmentname = department.departmentname,
                companyid = department.companyid,
                isactive = department.isactive,
                companyname = department.companyname, // If you're using it, else remove
                Companies = applicationdbcontext.Companies.Where(c => c.isactive).ToList()
            };

            return View(model);
        }
        [AdminOnly]
        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            
                // Repopulate dropdown before returning view
                department.Companies = await applicationdbcontext.Companies
                    .Where(c => c.isactive)
                    .ToListAsync();

             // return same model back to view with dropdown
            

            var existing = await applicationdbcontext.departments.FindAsync(department.id);
            if (existing == null)
                return NotFound();

            // Update only allowed properties
            existing.departmentname = department.departmentname;
            existing.companyid = department.companyid;
            existing.isactive = department.isactive;

            applicationdbcontext.departments.Update(existing);
            await applicationdbcontext.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        //[HttpPost]
        //public async Task<IActionResult> Edit(Department department)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var existingDept = await applicationdbcontext.departments.FindAsync(department.id);
        //        existingDept.companyid = department.companyid;
        //        existingDept.companyname = department.companyname;
        //        existingDept.departmentname = department.departmentname;
        //        existingDept.isactive = department.isactive;
        //        Department department1 = new Department()
        //        { 

        //        departmentname=department.departmentname,

        //        companyname=department.companyname,
        //        isactive=department.isactive==false,

        //        };

        //        applicationdbcontext.departments.Update(department1);
        //        await applicationdbcontext.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(department);
        //}
        [AdminOnly]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await applicationdbcontext.departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            department.isactive = false;
            applicationdbcontext.departments.Update(department);
            await applicationdbcontext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
