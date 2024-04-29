using Doctor_System.Data;
using Doctor_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Doctor_System.Data;
using Doctor_System.ViewModels;

namespace Doctor_System.Controllers
{
	public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);
            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }
            
            if (registerViewModel.Role == "Doctor")
            {
                var newUser = new Doctor()
                {
                    Email = registerViewModel.EmailAddress,
                    UserName = registerViewModel.EmailAddress,
                    Name = registerViewModel.Name,
                    Age = registerViewModel.Age,
                    PhoneNumber = registerViewModel.PhoneNumber,
                };
                var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);
                if (!newUserResponse.Succeeded){
                    TempData["Error"] = newUserResponse.Errors.First().Description;
                    return View(registerViewModel);
                }else{
                    await _userManager.AddToRoleAsync(newUser, registerViewModel.Role);
                }
            }else
            {
                var newUser = new Patient()
                {
                    Email = registerViewModel.EmailAddress,
                    UserName = registerViewModel.EmailAddress,
                    Name = registerViewModel.Name,
                    Age = registerViewModel.Age,
                    PhoneNumber = registerViewModel.PhoneNumber,
                };
                var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);
                if (!newUserResponse.Succeeded)
                {
                    TempData["Error"] = newUserResponse.Errors.First().Description;
                    return View(registerViewModel);
                }
                else
                {
                    await _userManager.AddToRoleAsync(newUser, registerViewModel.Role);
                }
            }


            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);
            if (user != null)
            {
                // User is found
                var PasswordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (PasswordCheck)
                {
                    // Password is correct sign in
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                // Password is incorrect
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(loginViewModel);
            }
            // User not found
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
