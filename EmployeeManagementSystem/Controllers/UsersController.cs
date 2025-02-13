using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    //[Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsersController> _logger;


        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager,
            ILogger<UsersController> logger)
        {

            this._userManager = userManager;

            this._roleManager = roleManager;

            this._signInManager = signInManager;

            this._context = context;
            this._logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    FullName = string.Join(" ", new[] { user.FirstName, user.MiddleName, user.LastName }
                                            .Where(name => !string.IsNullOrWhiteSpace(name))), // Fix nulls and spaces
                    NationalId = user.NationalId,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Role = roles.Any() ? string.Join(", ", roles) : "No Role Assigned"
                });
            }

            _logger.LogInformation($"Number of users retrieved: {userList.Count}");
            return View(userList);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            if (roles == null || roles.Count == 0)
            {
                _logger.LogWarning("No roles found in the database.");
            }

            ViewData["RoleId"] = new SelectList(roles, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Log validation errors
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        _logger.LogError($"Validation error: {key} - {error.ErrorMessage}");
                    }
                }

                // Repopulate Role Dropdown
                ViewData["RoleId"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", model.RoleId);
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                NationalId = model.NationalId,
                NormalizedUserName = model.UserName.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = true,
                CreatedOn = DateTime.Now,
                CreatedById = "Vinit Ahir",
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign role to the user
                if (!string.IsNullOrEmpty(model.RoleId))
                {
                    var role = await _roleManager.FindByIdAsync(model.RoleId);
                    if (role != null)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                }

                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    _logger.LogError($"User creation failed: {error.Description}");
                }
            }

            // Repopulate Role Dropdown
            ViewData["RoleId"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", model.RoleId);
            return View(model);
        }

        //Edit Method

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                NationalId = user.NationalId,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleId = (await _userManager.GetRolesAsync(user)).FirstOrDefault() // Get user role
            };

            ViewData["RoleId"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", model.RoleId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        _logger.LogError($"Validation error: {key} - {error.ErrorMessage}");
                    }
                }

                ViewData["RoleId"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", model.RoleId);
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            // Update user properties
            user.UserName = model.UserName;
            user.FirstName = model.FirstName;
            user.MiddleName = model.MiddleName;
            user.LastName = model.LastName;
            user.NationalId = model.NationalId;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.NormalizedUserName = model.UserName.ToUpper();
            user.NormalizedEmail = model.Email.ToUpper();

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Update user role
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                if (!string.IsNullOrEmpty(model.RoleId))
                {
                    var role = await _roleManager.FindByIdAsync(model.RoleId);
                    if (role != null)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                }

                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    _logger.LogError($"User update failed: {error.Description}");
                }
            }

            ViewData["RoleId"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", model.RoleId);
            return View(model);
        }





        //Delete method
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserViewModel
            {
                Id = user.Id,
                FullName = string.Join(" ", new[] { user.FirstName, user.MiddleName, user.LastName }
                                                    .Where(name => !string.IsNullOrWhiteSpace(name))),
                NationalId = user.NationalId,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Role = roles.Any() ? string.Join(", ", roles) : "No Role Assigned"
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User deleted successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }




        //public async Task<ActionResult> Index()
        //{
        //    var users = await _context.Users.ToListAsync();
        //    // Debugging: Log the number of users to check if data is coming through
        //    _logger.LogInformation($"Number of users retrieved: {users}");
        //    return View(users);
        //}

        //        //user  controller create method

        //        [HttpGet]
        //public async Task<ActionResult> Create()
        //{
        //    var roles = await _roleManager.Roles.ToListAsync();
        //    if (roles == null || roles.Count == 0)
        //    {
        //        _logger.LogWarning("No roles found in the database.");
        //    }

        //    ViewData["RoleId"] = new SelectList(roles, "Id", "Name");
        //    return View();
        //}


        //        [HttpPost]
        //        public async Task<ActionResult> Create(UserViewModel model)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                var user = new ApplicationUser
        //                {
        //                    UserName = model.UserName,
        //                    FirstName = model.FirstName,
        //                    MiddleName = model.MiddleName, // Fixed from LastName
        //                    LastName = model.LastName,
        //                    NationalId = model.NationalId,
        //                    NormalizedUserName = model.UserName.ToUpper(),
        //                    Email = model.Email,
        //                    NormalizedEmail = model.Email.ToUpper(),
        //                    EmailConfirmed = true,
        //                    PhoneNumber = model.PhoneNumber,
        //                    PhoneNumberConfirmed = true,
        //                    CreatedOn = DateTime.Now,
        //                    CreatedById = "Vinit Ahir",
        //                    RoleId = model.RoleId, // Ensure RoleId is assigned
        //                };

        //                // Create the user in the database
        //                var result = await _userManager.CreateAsync(user, model.Password);

        //                if (result.Succeeded)
        //                {
        //                    // Assign the role to the user
        //                    var role = await _roleManager.FindByIdAsync(model.RoleId);
        //                    if (role != null)
        //                    {
        //                        await _userManager.AddToRoleAsync(user, role.Name);
        //                    }

        //                    TempData["SuccessMessage"] = "User created successfully!";
        //                    return RedirectToAction("Index");
        //                }
        //                else
        //                {
        //                    foreach (var error in result.Errors)
        //                    {
        //                        ModelState.AddModelError("", error.Description);
        //                    }
        //                }
        //            }

        //            // Repopulate Role Dropdown
        //            ViewData["RoleId"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", model.RoleId);
        //            return View(model);
        //        }

    }
}
