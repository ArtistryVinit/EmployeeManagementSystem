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
            var users = await _context.Users.ToListAsync();
            // Debugging: Log the number of users to check if data is coming through
            _logger.LogInformation($"Number of users retrieved: {users.Count}");
            return View(users);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ViewData["RoleId"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name");
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName, // Fixed from LastName
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
                    RoleId = model.RoleId, // Ensure RoleId is assigned
                };

                // Create the user in the database
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign the role to the user
                    var role = await _roleManager.FindByIdAsync(model.RoleId);
                    if (role != null)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }

                    TempData["SuccessMessage"] = "User created successfully!";
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

            // Repopulate Role Dropdown
            ViewData["RoleId"] = new SelectList(await _roleManager.Roles.ToListAsync(), "Id", "Name", model.RoleId);
            return View(model);
        }

    }
}
