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

		public async Task<IActionResult> Clinic()
		{
			var current = await _userManager.GetUserAsync(User);
			if (current == null)
			{
				return RedirectToAction("Index", "Home");
			}

            var clinic = _context.Clinics.FirstOrDefault(cli => cli.DoctorId == current.Id);
			if (clinic == null)
			{
				return RedirectToAction("Index", "Home");
			}
			var workingHours = _context.ClinicsWorkingHours.Where(cwh => cwh.ClinicId == clinic.Id).ToList();
			if (workingHours == null)
			{
				return RedirectToAction("Index", "Home");
			}

			var model = new ClinicViewModel()
            {
                Clinic = clinic,
                Doctor = clinic.Doctor,
                ClinicWorkingHours = workingHours
			};

			return View(model);
		}
        public IActionResult AddClinic()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddClinic(AddClinicViewModel addClinicViewModel)
        {
            if (!ModelState.IsValid) return View(addClinicViewModel);
            Clinic clinic = new()
            {
                DoctorId = addClinicViewModel.DoctorId,
                Email = addClinicViewModel.EmailAddress,
                Phone = addClinicViewModel.PhoneNumber,
                Address = addClinicViewModel.Address,
                Price = addClinicViewModel.Price,
            };

            var newClinic = _context.Clinics.Add(clinic);
            _context.SaveChanges();
            return RedirectToAction("Clinic");
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

        [HttpGet]
        public IActionResult DeleteSpec(int Id) {
            var spec = _context.DoctorsSpecializations.FirstOrDefault(x => x.Id == Id);
            if(spec != null)
            {
                _context.Remove(spec);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Profile");
        }

        public IActionResult AddWorkingHours()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddWorkingHours(AddWorkingHoursViewModel addWorkingHoursViewModel)
        {
            if (!ModelState.IsValid) return View(addWorkingHoursViewModel);
            var current = await _userManager.GetUserAsync(User);
            if (current == null) return RedirectToAction("Index", "Home");

            var clinic = _context.Clinics.FirstOrDefault(cli => cli.DoctorId == current.Id);
            if(clinic == null) return RedirectToAction("Index", "Home");

            ClinicWorkingHours newWorkingHours = new()
            {
                ClinicId = clinic.Id,
                DayOfWeek = addWorkingHoursViewModel.DayOfWeek,
                ClosingTime = addWorkingHoursViewModel.ClosingTime,
                OpeningTime = addWorkingHoursViewModel.OpeningTime,
            };

            var successed = _context.ClinicsWorkingHours.Add(newWorkingHours);
            if (successed == null) return View(addWorkingHoursViewModel);
            _context.SaveChanges();

            return RedirectToAction("Clinic");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteWorkingHours(string Day)
        {
			var current = await _userManager.GetUserAsync(User);
			if (current == null)
			{
				return RedirectToAction("Index", "Home");
			}
			var clinic = _context.Clinics.FirstOrDefault(cli => cli.DoctorId == current.Id);
			if (clinic == null)
			{
				return RedirectToAction("Index", "Home");
			}
			var workingHours = _context.ClinicsWorkingHours.FirstOrDefault(cwh => cwh.ClinicId == clinic.Id && cwh.DayOfWeek == Day);
			if (workingHours == null)
			{
				return RedirectToAction("Index", "Home");
			}

			_context.Remove(workingHours);
			_context.SaveChanges();

            return RedirectToAction("Clinic");
		}


		public IActionResult Appointments()
        {
            return View();
        }

	}
}
