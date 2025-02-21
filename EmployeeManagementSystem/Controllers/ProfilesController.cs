using EmployeeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmployeeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EmployeeManagementSystem.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = new ProfileViewModel();
            var roles = await _context.Roles.OrderBy(x => x.Name).ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            tasks.Profiles = await _context.systemProfiles
            .Include(t => t.Children)
            .ThenInclude(c => c.Children)
            .ThenInclude(gc => gc.Children)
            .OrderBy(x => x.Order)
            .ToListAsync();


            ViewBag.Tasks = new SelectList(tasks.Profiles, "Id", "Name"); // ✅ Correct


            return View(tasks);
        }

        public async Task<IActionResult> AssignRights(ProfileViewModel vm)
        {
            var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = new RoleProfile
            {
                TaskId = vm.TaskId,
                RoleId = vm.RoleId
            };

            _context.RoleProfiles.Add(role);
            await _context.SaveChangesAsync(Userid);
            return RedirectToAction("Index");

        }


        [HttpGet]
        public async Task<IActionResult> UserRights(string id)
        {
            var tasks = new ProfileViewModel();
            tasks.RoleId = id;
            var systemtasks = await _context.systemProfiles
                .Include(s => s.Profile) // ✅ Include Profile navigation property
                .Include(s => s.Children) // ✅ Include first level of children
                    .ThenInclude(c => c.Children) // ✅ Second level of children
                        .ThenInclude(c => c.Children) // ✅ Third level of children
                .OrderBy(x => x.Order)
                .ToListAsync();

            tasks.Profiles = systemtasks;

            tasks.RolesProfilesIds = await _context.RoleProfiles
                .Where(x => x.RoleId == id)
                .Select(r => r.TaskId)
                .ToListAsync();

            ViewBag.Tasks = new SelectList(systemtasks, "Id", "Name"); // ✅ Correct

            return View(tasks);
        }
        [HttpPost]
        public async Task<IActionResult> UserGroupRights(string id, ProfileViewModel vm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if vm.Ids has values
            if (vm.Ids == null || !vm.Ids.Any())
            {
                ModelState.AddModelError("", "No tasks selected.");
                return View(vm); // Return view with validation message
            }

            foreach (var taskId in vm.Ids)
            {
                var roleProfile = new RoleProfile
                {
                    TaskId = taskId,
                    RoleId = id
                };

                _context.RoleProfiles.Add(roleProfile);
            }

            await _context.SaveChangesAsync(); // ✅ Corrected SaveChangesAsync

            return RedirectToAction("UserRights", new { id }); // ✅ Redirect to avoid duplicate form submission
        }

    }

}












//try code controller
//using EmployeeManagementSystem.Data;
//using EmployeeManagementSystem.Models;
//using EmployeeManagementSystem.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace EmployeeManagementSystem.Controllers
//{
//    public class ProfilesController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public ProfilesController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var viewModel = new ProfileViewModel
//            {
//                Profiles = await _context.systemProfiles
//                    .Include(t => t.Children)
//                    .ThenInclude(c => c.Children)
//                    .ThenInclude(gc => gc.Children)
//                    .OrderBy(x => x.Order)
//                    .ToListAsync()
//            };

//            ViewBag.Roles = new SelectList(await _context.Roles.OrderBy(x => x.Name).ToListAsync(), "Id", "Name");
//            ViewBag.Tasks = new SelectList(viewModel.Profiles, "Id", "Name");

//            return View(viewModel);
//        }

//        [HttpPost]
//        public async Task<IActionResult> AssignRights(ProfileViewModel vm)
//        {
//            if (vm.TaskId == null || vm.RoleId == null)
//            {
//                ModelState.AddModelError("", "Task and Role must be selected.");
//                return RedirectToAction("Index");
//            }

//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var roleProfile = new RoleProfile
//            {
//                TaskId = vm.TaskId,
//                RoleId = vm.RoleId
//            };

//            _context.RoleProfiles.Add(roleProfile);
//            await _context.SaveChangesAsync(); // ✅ Removed parameter (not needed)

//            return RedirectToAction("Index");
//        }

//        [HttpGet]
//        public async Task<IActionResult> UserRights(string id)
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                return BadRequest("Role ID is required.");
//            }

//            var systemProfiles = await _context.systemProfiles
//                .Include(s => s.Profile)
//                .Include(s => s.Children)
//                .ThenInclude(c => c.Children)
//                .ThenInclude(gc => gc.Children)
//                .OrderBy(x => x.Order)
//                .ToListAsync();

//            var viewModel = new ProfileViewModel
//            {
//                RoleId = id,
//                Profiles = systemProfiles,
//                RolesProfilesIds = await _context.RoleProfiles
//                    .Where(x => x.RoleId == id)
//                    .Select(r => r.TaskId)
//                    .ToListAsync()
//            };

//            ViewBag.Tasks = new SelectList(systemProfiles, "Id", "Name");

//            return View(viewModel);
//        }

//        [HttpPost]
//        public async Task<IActionResult> UserGroupRights(string id, ProfileViewModel vm)
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                return BadRequest("Role ID is required.");
//            }

//            if (vm.Ids == null || !vm.Ids.Any())
//            {
//                ModelState.AddModelError("", "No tasks selected.");
//                return View(vm);
//            }

//            var existingRoleProfiles = await _context.RoleProfiles
//                .Where(rp => rp.RoleId == id)
//                .ToListAsync();

//            _context.RoleProfiles.RemoveRange(existingRoleProfiles);

//            var newRoleProfiles = vm.Ids.Select(taskId => new RoleProfile
//            {
//                TaskId = taskId,
//                RoleId = id
//            });

//            await _context.RoleProfiles.AddRangeAsync(newRoleProfiles);
//            await _context.SaveChangesAsync();

//            return RedirectToAction("UserRights", new { id });
//        }
//    }
//}


