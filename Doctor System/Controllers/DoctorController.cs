using Doctor_System.Data;
using Doctor_System.Models;
using Doctor_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Doctor_System.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DoctorController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult AddSpec()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSpec(IFormCollection req)
        {
            if (!ModelState.IsValid) return View();
            var docId = req["DoctorId"];
            var Specialization = req["Specialization"];

            if(string.IsNullOrEmpty(Specialization))
            {
                return View();
            }

            DoctorSpecialization doctorSpecialization = new()
            {
                DoctorId = docId,
                Specialization = Specialization
            };

            var newSpecialization = _context.DoctorsSpecializations.Add(doctorSpecialization);
            _context.SaveChanges();
            return RedirectToAction("Index", "Profile");
        }
    }
}
