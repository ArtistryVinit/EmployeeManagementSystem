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
    public class LeaveApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeaveApplications
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.leaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LeaveApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.leaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        //    // GET: LeaveApplications/Create
        //    public IActionResult Create()
        //    {
        //        ViewData["DurationId"] = new SelectList(_context.systemCodeDetails.Include(x=>x.SystemCode).Where(y=>y.SystemCode.Code== "LeaveDuration"), "Id", "Description");
        //        ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
        //        ViewData["LeaveTypeId"] = new SelectList(_context.leaveTypes, "Id", "Name");

        //        return View();
        //    }

        //    // POST: LeaveApplications/Create

        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Create(LeaveApplication leaveApplication)
        //    {
        //        //var pendingstatus = _context.systemCodeDetails.Include(x => x.SystemCode).Where(y => y.Code == "pending" && y.SystemCode.Code== "Leave Approval Status").FirstOrDefaultAsync();
        //        var pendingstatus = await _context.systemCodeDetails
        //.Include(x => x.SystemCode)
        //.Where(y => y.Code == "pending" && y.SystemCode.Code == "Leave Approval Status")
        //.FirstOrDefaultAsync();

        //        if (ModelState.IsValid)
        //        {
        //            leaveApplication.CreatedOn = DateTime.Now;
        //            leaveApplication.CreatedById = "Vinit Ahir";
        //            leaveApplication.StatusId = pendingstatus.Id;
        //            _context.Add(leaveApplication);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewData["DurationId"] = new SelectList(_context.systemCodeDetails.Include(x=>x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description", leaveApplication.DurationId);
        //        ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
        //        ViewData["LeaveTypeId"] = new SelectList(_context.leaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);

        //        return View(leaveApplication);
        //    }

        // GET: LeaveApplications/Create
        public IActionResult Create()
        {
            PopulateViewData(); // Load dropdown data
            return View();
        }

        // POST: LeaveApplications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveApplication leaveApplication)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewData(leaveApplication);
                return View(leaveApplication);
            }

            try
            {
                // Fetch the "pending" status from the system codes
                var pendingStatus = await _context.systemCodeDetails
                    .Include(x => x.SystemCode)
                    .Where(y => y.Code == "pending" && y.SystemCode.Code == "Leave Approval Status")
                    .FirstOrDefaultAsync();

                if (pendingStatus == null)
                {
                    ModelState.AddModelError("", "Pending status not found in the system.");
                    PopulateViewData(leaveApplication);
                    return View(leaveApplication);
                }

                // Assign required values before saving
                leaveApplication.CreatedOn = DateTime.Now;
                leaveApplication.CreatedById = "Vinit Ahir"; // Replace with logged-in user ID
                leaveApplication.StatusId = pendingStatus.Id;

                // Save the leave application
                _context.Add(leaveApplication);
                await _context.SaveChangesAsync();

                // Success message and redirect to the Index page
                TempData["SuccessMessage"] = "Leave application submitted successfully!";
                return RedirectToAction("Index"); // Redirect to Index.cshtml
            }
            catch (DbUpdateException dbEx)
            {
                ModelState.AddModelError("", "Database error: Unable to save data.");
                Console.WriteLine($"Database Error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                Console.WriteLine($"General Error: {ex.Message}");
            }

            // If an error occurs, reload ViewData and return the view
            PopulateViewData(leaveApplication);
            return View(leaveApplication);
        }

        // Method to populate ViewData for dropdowns
        private void PopulateViewData(LeaveApplication leaveApplication = null)
        {
            ViewData["DurationId"] = new SelectList(_context.systemCodeDetails
                .Include(x => x.SystemCode)
                .Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description", leaveApplication?.DurationId);

            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication?.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.leaveTypes, "Id", "Name", leaveApplication?.LeaveTypeId);
        }


        // GET: LeaveApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.leaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            ViewData["DurationId"] = new SelectList(_context.systemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.leaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);
            
            return View(leaveApplication);
        }

        // POST: LeaveApplications/Edit/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var pendingstatus = _context.systemCodeDetails.Include(x => x.SystemCode).Where(y => y.Code == "pending" && y.SystemCode.Code == "Leave Approval Status").FirstOrDefaultAsync();

                try
                {
                   

                    leaveApplication.ModifiedOn = DateTime.Now;
                    leaveApplication.ModifiedById = "Vinit Ahir";
                    leaveApplication.StatusId = pendingstatus.Id;
                    _context.Update(leaveApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.id))
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
            ViewData["DurationId"] = new SelectList(_context.systemCodeDetails, "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.leaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);
            
            return View(leaveApplication);
        }


        // GET: LeaveApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.leaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // POST: LeaveApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveApplication = await _context.leaveApplications.FindAsync(id);
            if (leaveApplication != null)
            {
                _context.leaveApplications.Remove(leaveApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveApplicationExists(int id)
        {
            return _context.leaveApplications.Any(e => e.id == id);
        }
    }
}
