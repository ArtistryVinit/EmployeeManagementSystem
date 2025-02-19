using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var leaveApplications = await _context.leaveApplications
                .Include(l => l.Employee)  // Ensure Employee data is included
                .Include(l => l.LeaveType) // Ensure LeaveType data is included
                .Include(l => l.Status)    // Ensure Status data is included
                .ToListAsync();

            return View(leaveApplications);
        }

        //this is leave application create method.
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["LeaveTypeId"] = new SelectList(_context.leaveTypes, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,LeaveTypeId,NumberOfDays,StartDate,EndDate,Description,Attachment")] LeaveApplication leaveApplication)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Add leave application to database
                    _context.Add(leaveApplication);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Leave application submitted successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving data: " + ex.Message);
                }
            }

            // Repopulate dropdowns if validation fails
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.leaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);

            return View(leaveApplication);
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

            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.leaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);
            ViewData["StatusId"] = new SelectList(_context.systemCodeDetails, "Id", "Name", leaveApplication.StatusId);

            return View(leaveApplication);
        }

        // POST: LeaveApplications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,LeaveTypeId,NumberOfDays,StartDate,EndDate,Description,Attachment,StatusId")] LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.Id))
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

            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.leaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);
            ViewData["StatusId"] = new SelectList(_context.systemCodeDetails, "Id", "Name", leaveApplication.StatusId);

            return View(leaveApplication);
        }

        private bool LeaveApplicationExists(int id)
        {
            return _context.leaveApplications.Any(e => e.Id == id);
        }

        // GET: LeaveApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.leaveApplications
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

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
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);

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
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LeaveApplications/ApproveLeave/5
        public async Task<IActionResult> ApproveLeave(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.leaveApplications
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // POST: LeaveApplications/ApproveLeave/5
        [HttpPost, ActionName("ApproveLeave")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveLeaveConfirmed(int id)
        {
            var leaveApplication = await _context.leaveApplications.FindAsync(id);

            if (leaveApplication != null)
            {
                // Assuming 'Approved' status has an ID of 2 in the SystemCodeDetails table
                leaveApplication.StatusId = 2; // Change this based on your actual status ID for "Approved"
                _context.Update(leaveApplication);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LeaveApplications/ApprovedLeaveApplications
        public async Task<IActionResult> ApprovedLeaveApplications()
        {
            // Assuming 'Approved' status has an ID of 2 in the systemCodeDetails table
            var approvedLeaves = await _context.leaveApplications
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .Where(l => l.StatusId == 2) // Change '2' to your actual approved status ID
                .ToListAsync();

            return View(approvedLeaves);
        }

        // GET: LeaveApplications/RejectLeave/5
        public async Task<IActionResult> RejectLeave(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.leaveApplications
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // POST: LeaveApplications/RejectLeave/5
        [HttpPost, ActionName("RejectLeave")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectLeaveConfirmed(int id)
        {
            var leaveApplication = await _context.leaveApplications.FindAsync(id);

            if (leaveApplication != null)
            {
                // Assuming 'Rejected' status has an ID of 3 in the SystemCodeDetails table
                leaveApplication.StatusId = 3; // Change this based on your actual status ID for "Rejected"
                _context.Update(leaveApplication);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LeaveApplications/RejectedLeaveApplications
        public async Task<IActionResult> RejectedLeaveApplications()
        {
            // Assuming 'Rejected' status has an ID of 3
            var rejectedLeaves = await _context.leaveApplications
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .Where(l => l.StatusId == 3) // Change '3' to your actual rejected status ID
                .ToListAsync();

            return View(rejectedLeaves);
        }
        

    }
}
