using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamTakingApp.Models;
using ExamTakingApp.Models.Enums;
using ExamTakingApp.ViewModels.Auth;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExamTakingApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        // GET: Account/Register
        public IActionResult RegisterForLevelExam()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterForLevelExam(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            try
            {
                // Validate phone number uniqueness
                if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == registerVM.PhoneNumber))
                {
                    ModelState.AddModelError(nameof(RegisterVM.PhoneNumber), "This phone number is already registered.");
                    return View(registerVM);
                }

                var appUser = new AppUser
                {
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName,
                    UserName = Guid.NewGuid().ToString(), 
                    Email = Guid.NewGuid().ToString(),
                    PhoneNumber = registerVM.PhoneNumber,
                };

                var createResult = await _userManager.CreateAsync(appUser, Guid.NewGuid().ToString());
                if (!createResult.Succeeded)
                {
                    return ReturnWithErrors(registerVM, createResult.Errors);
                }

                const string studentRole = nameof(RolesEnum.Student);
                if (!await _roleManager.RoleExistsAsync(studentRole))
                {
                    var roleCreateResult = await _roleManager.CreateAsync(new IdentityRole(studentRole));
                    if (!roleCreateResult.Succeeded)
                    {
                        return ReturnWithErrors(registerVM, roleCreateResult.Errors);
                    }
                }

                var addRoleResult = await _userManager.AddToRoleAsync(appUser, studentRole);
                if (!addRoleResult.Succeeded)
                {
                    return ReturnWithErrors(registerVM, addRoleResult.Errors);
                }

                // Sign in the user after successful registration
                TempData["SuccessMessage"] = "Registration successful.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log exception (implement proper logging in production)
                ModelState.AddModelError("", "An error occurred during registration.");
                return View(registerVM);
            }
        }
        public IActionResult RegisterForSatExam()
        {
            return View();
        }
                // POST: Account/Register
        [HttpPost]
        public async Task<IActionResult> RegisterForSatExam(RegisterSatExamVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            try
            {
                // Validate phone number uniqueness
                if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == registerVM.PhoneNumber))
                {
                    ModelState.AddModelError(nameof(RegisterVM.PhoneNumber), "This phone number is already registered.");
                    return View(registerVM);
                }

                var appUser = new AppUser
                {
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName,
                    UserName = Guid.NewGuid().ToString(), 
                    Email = Guid.NewGuid().ToString(),
                    PhoneNumber = registerVM.PhoneNumber,
                };

                var createResult = await _userManager.CreateAsync(appUser, Guid.NewGuid().ToString());
                if (!createResult.Succeeded)
                {
                    return ReturnWithErrorsRegisterSat(registerVM, createResult.Errors);
                }

                const string studentRole = nameof(RolesEnum.Student);
                if (!await _roleManager.RoleExistsAsync(studentRole))
                {
                    var roleCreateResult = await _roleManager.CreateAsync(new IdentityRole(studentRole));
                    if (!roleCreateResult.Succeeded)
                    {
                        return ReturnWithErrorsRegisterSat(registerVM, roleCreateResult.Errors);
                    }
                }

                var addRoleResult = await _userManager.AddToRoleAsync(appUser, studentRole);
                if (!addRoleResult.Succeeded)
                {
                    return ReturnWithErrorsRegisterSat(registerVM, addRoleResult.Errors);
                }

                // Sign in the user after successful registration
                await _signInManager.SignInAsync(appUser, isPersistent: false);
                TempData["SuccessMessage"] = "Registration successful.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log exception (implement proper logging in production)
                ModelState.AddModelError("", "An error occurred during registration.");
                return View(registerVM);
            }
        }
      
        // GET: Account/Login
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            try
            {
                var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == loginVM.PhoneNumber.Trim());

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid phone number or password.");
                    return View(loginVM);
                }

                if (user.UserType == UserType.LevelTestUser)
                {
                    ModelState.AddModelError("", "you cant login level test user");
                    return View(loginVM);
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName,
                    loginVM.Password,
                    loginVM.RememberMe,
                    lockoutOnFailure: true);

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Your account is temporarily locked due to multiple failed login attempts.");
                    return View(loginVM);
                }

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Invalid phone number or password.");
                    return View(loginVM);
                }

                TempData["SuccessMessage"] = "Login successful.";
                return LocalRedirect(returnUrl ?? Url.Action("Index", "Home"));
            }
            catch (Exception ex)
            {
                // Log exception (implement proper logging in production)
                ModelState.AddModelError("", "An error occurred during login.");
                return View(loginVM);
            }
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            try
            {
                await _signInManager.SignOutAsync();
                TempData["SuccessMessage"] = "Logged out successfully.";
                return LocalRedirect(returnUrl ?? Url.Action("Index", "Home"));
            }
            catch (Exception ex)
            {
                // Log exception (implement proper logging in production)
                TempData["ErrorMessage"] = "An error occurred during logout.";
                return RedirectToAction("Index", "Home");
            }
        }

        private IActionResult ReturnWithErrors(RegisterVM model, IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
        private IActionResult ReturnWithErrorsRegisterSat(RegisterSatExamVM model, IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
    }
}