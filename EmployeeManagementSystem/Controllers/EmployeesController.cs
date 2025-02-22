using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;
        public EmployeesController(IConfiguration configuration,ApplicationDbContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        //Details code
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Gender)      // Include Gender details
                .Include(e => e.Department)  // Include Department details
                .Include(e => e.Designation) // Include Designation details
                .Include(e => e.Country)     // Include Country details
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }


        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["BankId"] = new SelectList(_context.banks, "Id", "Name");
            ViewData["EmploymentTermsId"] = new SelectList(_context.systemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "EmploymentTerms"), "Id", "Description");

            ViewData["GenderId"] = new SelectList(_context.systemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Gender"), "Id", "Description");
            ViewData["CountryId"] = new SelectList(_context.countries, "Id", "Name");
            ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Employees/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee,IFormFile employeephoto)
        {
            if (employeephoto != null && employeephoto.Length > 0)
            {
                var filename = "EmployeePhoto_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(employeephoto.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "EmployeePicture");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, filename);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await employeephoto.CopyToAsync(stream);
                }

                employee.Photo = filename; // Save only the filename
            }




            employee.CreatedById = "Vinit Ahir";
            employee.CreatedOn = DateTime.Now;



            _context.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["BankId"] = new SelectList(_context.banks, "Id", "Name",employee.BankId);
            ViewData["EmploymentTermsId"] = new SelectList(_context.systemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "EmploymentTerms"), "Id", "Description",employee.BankId);

            ViewData["GenderId"] = new SelectList(_context.systemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Gender"), "Id", "Description");
            ViewData["CountryId"] = new SelectList(_context.countries, "Id", "Name", employee.CountryId);
            ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name", employee.DesignationId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            ViewData["GenderId"] = new SelectList(_context.systemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Gender"), "Id", "Description");
            ViewData["CountryId"] = new SelectList(_context.countries, "Id", "Name", employee.CountryId);
            ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name", employee.DesignationId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        // POST: Employees/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmpNo,FirstName,MiddleName,LastName,PhoneNumber,EmailAddress,Country,DateOfBirth,Address,Department,Designation,CreatedById,CreatedOn,ModifiedById,ModifiedOn")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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

            ViewData["GenderId"] = new SelectList(_context.systemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Gender"), "Id", "Description");
            ViewData["CountryId"] = new SelectList(_context.countries, "Id", "Name", employee.CountryId);
            ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name", employee.DesignationId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
