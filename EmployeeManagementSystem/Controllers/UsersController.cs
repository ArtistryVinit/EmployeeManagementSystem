using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    //[Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsersController> _logger;


        public UsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager,
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
           
            
            // var users = await _context.Users.ToListAsync();
            // Debugging: Log the number of users to check if data is coming through
            //_logger.LogInformation($"Number of users retrieved: {users.Count}");
            

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new IdentityUser object
                IdentityUser user = new IdentityUser
                {
                    UserName = model.UserName,
                    NormalizedUserName = model.UserName.ToUpper(),
                    Email = model.Email,
                    EmailConfirmed = true,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = true
                };

                // Create the user in the database
                var result = await _userManager.CreateAsync(user, model.Password);

                // Check if the user creation was successful
                if (result.Succeeded)
                {
                    // Optionally add a role or other properties here
                    // await _userManager.AddToRoleAsync(user, "User");

                    // Redirect to the Index action or a success page
                    TempData["SuccessMessage"] = "User created successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Add validation errors if user creation fails
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            // If we got here, something went wrong, so show the form again
            return View(model);
        }
    }
}
