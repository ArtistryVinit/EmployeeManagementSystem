using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EmployeeManagementSystem.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RolesController> _logger;

        public RolesController(RoleManager<IdentityRole> roleManager, ILogger<RolesController> logger)
        {
            this._roleManager = roleManager;
            this._logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        //Roller create method
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(RolesViewModel model)
        {
            _logger.LogInformation("Checking if RoleManager is initialized.");
            if (_roleManager == null)
            {
                _logger.LogError("RoleManager is NULL.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                // Check if role already exists
                if (await _roleManager.RoleExistsAsync(model.RoleName))
                {
                    ModelState.AddModelError("", "Role already exists.");
                    return View(model);
                }

                IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };

                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role created successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }

        //Roller Edit method

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError("Edit Role: Role ID is NULL.");
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                _logger.LogError($"Edit Role: Role with ID '{id}' not found.");
                return NotFound();
            }

            var model = new RolesViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RolesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                return NotFound();
            }

            if (await _roleManager.RoleExistsAsync(model.RoleName) && role.Name != model.RoleName)
            {
                ModelState.AddModelError("", "Role name already exists.");
                return View(model);
            }

            role.Name = model.RoleName;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Role updated successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        //Delete Method

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError("Delete Role: Role ID is NULL.");
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                _logger.LogError($"Delete Role: Role with ID '{id}' not found.");
                return NotFound();
            }

            var model = new RolesViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                _logger.LogError($"Delete Role: Role with ID '{id}' not found.");
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Role deleted successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View("Delete", new RolesViewModel { Id = id, RoleName = role.Name });
        }

        //Roles Details Method

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogError("Details Role: Role ID is NULL.");
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                _logger.LogError($"Details Role: Role with ID '{id}' not found.");
                return NotFound();
            }

            var model = new RolesViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            return View(model); // Let ASP.NET locate Details.cshtml automatically
        }


    }
}




