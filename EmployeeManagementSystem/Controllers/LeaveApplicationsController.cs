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
    public class LeaveApplicationsController(ApplicationDbContext context, ILogger<LeaveApplicationsController> logger) : Controller
    {
        private readonly ILogger<LeaveApplicationsController> _logger = logger;


        // GET: LeaveApplications
        public async Task<IActionResult> Index()
        {
            var awaitingstatus = context.systemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "AwaitingApproval").FirstOrDefault();

            var applicationDbContext = context.leaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .Where(l => l.StatusId == awaitingstatus!.Id);

            return View(await applicationDbContext.ToListAsync());
        }

        //Approved Application
        public async Task<IActionResult> ApprovedLeaveApplications()
        {
            var approvedstatus = context.systemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Approved").FirstOrDefault();

            var applicationDbContext = context.leaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .Where(l => l.StatusId == approvedstatus!.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        //RejectedLeaveApplications

        public async Task<IActionResult> RejectedLeaveApplications()
        {
            var Rejectedstatus = context.systemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Rejected").FirstOrDefault();

            var applicationDbContext = context.leaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                 .Where(l => l.StatusId == Rejectedstatus!.Id);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: LeaveApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await context.leaveApplications
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

        [HttpGet]
        public async Task<IActionResult> RejectLeave(int? id)
        {
            var leaveApplication = await context.leaveApplications
            .Include(l => l.Duration)
            .Include(l => l.Employee)
            .Include(l => l.LeaveType)
            .Include(l => l.Status)
            .FirstOrDefaultAsync(m => m.id == id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            ViewData["DurationId"] = new SelectList(context.systemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "Rejected"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(context.Employees, "Id", "FullName");
            ViewData["LeaveTypeId"] = new SelectList(context.leaveTypes, "Id", "Name");

            return View(leaveApplication);
        }

        [HttpPost]
        public async Task<IActionResult> RejectLeave(LeaveApplication leave)
        {
            var Rejectstatus = context.systemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Approved").FirstOrDefault();

            var leaveApplication = await context.leaveApplications
            .Include(l => l.Duration)
            .Include(l => l.Employee)
            .Include(l => l.LeaveType)
            .Include(l => l.Status)
            .FirstOrDefaultAsync(m => m.id == leave.id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            leaveApplication.ApprovedOn = DateTime.Now;
            leaveApplication.ApprovedById = "Vinit Ahir";
            leaveApplication.StatusId = Rejectstatus.Id;

            context.Update(leaveApplication);
            await context.SaveChangesAsync();

            ViewData["DurationId"] = new SelectList(context.systemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(context.Employees, "Id", "FullName");
            ViewData["LeaveTypeId"] = new SelectList(context.leaveTypes, "Id", "Name");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ApproveLeave(int? id)
        {
            var leaveApplication = await context.leaveApplications
            .Include(l => l.Duration)
            .Include(l => l.Employee)
            .Include(l => l.LeaveType)
            .Include(l => l.Status)
            .FirstOrDefaultAsync(m => m.id == id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            ViewData["DurationId"] = new SelectList(context.systemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(context.Employees, "Id", "FullName");
            ViewData["LeaveTypeId"] = new SelectList(context.leaveTypes, "Id", "Name");

            return View(leaveApplication);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveLeave(LeaveApplication leave)
        {
            var approvedstatus = context.systemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Approved").FirstOrDefault();

            var leaveApplication = await context.leaveApplications
            .Include(l => l.Duration)
            .Include(l => l.Employee)
            .Include(l => l.LeaveType)
            .Include(l => l.Status)
            .FirstOrDefaultAsync(m => m.id == leave.id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            leaveApplication.ApprovedOn = DateTime.Now;
            leaveApplication.ApprovedById = "Vinit Ahir";
            leaveApplication.StatusId = approvedstatus.Id;

            context.Update(leaveApplication);
            await context.SaveChangesAsync();

            ViewData["DurationId"] = new SelectList(context.systemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(context.Employees, "Id", "FullName");
            ViewData["LeaveTypeId"] = new SelectList(context.leaveTypes, "Id", "Name");

            return RedirectToAction(nameof(Index));
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
        private void PopulateViewData()
        {
            ViewBag.EmployeeId = new SelectList(context.Employees, "Id", "FirstName");
            ViewBag.DurationId = new SelectList(context.systemCodeDetails
                .Where(s => s.SystemCode.Code == "LeaveDuration"), "Id", "Description");
            ViewBag.LeaveTypeId = new SelectList(context.leaveTypes, "Id", "Name");
        }

        // POST: LeaveApplications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveApplication leaveApplication)
        {
            if (!ModelState.IsValid)
            {
                // Log ModelState errors but do not add to ModelState
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        _logger.LogError($"ModelState Error for {key}: {error.ErrorMessage}");
                    }
                }

                PopulateViewData(leaveApplication);
                return View(leaveApplication);
            }

            try
            {
                var pendingStatus = await context.systemCodeDetails
                    .Include(x => x.SystemCode)
                    .Where(y => y.Code == "pending" && y.SystemCode.Code == "Leave Approval Status")
                    .FirstOrDefaultAsync();

                if (pendingStatus == null)
                {
                    _logger.LogError("Pending status not found in the system."); // Log the error only
                    PopulateViewData(leaveApplication);
                    return View(leaveApplication);
                }

                leaveApplication.CreatedOn = DateTime.Now;
                leaveApplication.CreatedById = "Vinit Ahir"; // Replace with logged-in user ID
                leaveApplication.StatusId = pendingStatus.Id;

                context.Add(leaveApplication);
                await context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Leave application submitted successfully!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError($"Database Error: {dbEx.Message}"); // Log error but do not show in ModelState
            }
            catch (Exception ex)
            {
                _logger.LogError($"General Error: {ex.Message}"); // Log error but do not show in ModelState
            }

            PopulateViewData(leaveApplication);
            return View(leaveApplication);
        }


        // Method to populate ViewData for dropdowns
        private void PopulateViewData(LeaveApplication leaveApplication)
        {
            ViewBag.EmployeeId = new SelectList(context.Employees, "Id", "FirstName", leaveApplication?.EmployeeId);
            ViewBag.DurationId = new SelectList(context.systemCodeDetails
                .Where(s => s.SystemCode.Code == "LeaveDuration"), "Id", "Description", leaveApplication?.DurationId);
            ViewBag.LeaveTypeId = new SelectList(context.leaveTypes, "Id", "Name", leaveApplication?.LeaveTypeId);
        }



        // GET: LeaveApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await context.leaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            ViewData["DurationId"] = new SelectList(context.systemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(context.leaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);

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
                var pendingstatus = context.systemCodeDetails.Include(x => x.SystemCode).Where(y => y.Code == "pending" && y.SystemCode.Code == "Leave Approval Status").FirstOrDefaultAsync();

                try
                {


                    leaveApplication.ModifiedOn = DateTime.Now;
                    leaveApplication.ModifiedById = "Vinit Ahir";
                    leaveApplication.StatusId = pendingstatus.Id;
                    context.Update(leaveApplication);
                    await context.SaveChangesAsync();
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
            ViewData["DurationId"] = new SelectList(context.systemCodeDetails, "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(context.leaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);

            return View(leaveApplication);
        }


        // GET: LeaveApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await context.leaveApplications
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
            var leaveApplication = await context.leaveApplications.FindAsync(id);
            if (leaveApplication != null)
            {
                context.leaveApplications.Remove(leaveApplication);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveApplicationExists(int id)
        {
            return context.leaveApplications.Any(e => e.id == id);
        }
    }
}
