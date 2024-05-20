using Doctor_System.Data;
using Doctor_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doctor_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ApplicationDbContext _context;

		public AdminController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_context = context;
		}
		public IActionResult Index()
        {
            return View();
        }

		public IActionResult AllUsers()
		{
			var users = _context.Users.ToList();
			return View(users);
		}

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == Id);
            if (user != null)
            {
                if(await _userManager.IsInRoleAsync(user, "Doctor"))
                {
                    var clinic = _context.Clinics.FirstOrDefault(cli => cli.DoctorId == Id);
                    if (clinic != null)
                    {
                        _context.Remove(clinic);
                    }
                }
                _context.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Profile");
        }
    }
}
