using Doctor_System.Data;
using Doctor_System.Models;
using Doctor_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
                return View();
			}
			var workingHours = _context.ClinicsWorkingHours.Where(cwh => cwh.ClinicId == clinic.Id).ToList();

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
            var doc = _context.Doctors.FirstOrDefault(doc => doc.Id == addClinicViewModel.DoctorId);
            Clinic clinic = new()
            {
                DoctorId = addClinicViewModel.DoctorId,
                Email = addClinicViewModel.EmailAddress,
                Phone = doc.PhoneNumber,
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
                TempData["Error"] = "Specialization is required";
                return View();
            }
            var existSpec = _context.DoctorsSpecializations.FirstOrDefault(sp => (sp.DoctorId == docId.ToString() && sp.Specialization == Specialization.ToString()));
            if (existSpec != null) {
                TempData["Error"] = "This Specialization is already exist";
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

        public IActionResult DoctorAppointments()
        {
            // Assuming you have the current user's ID from your authentication system
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch appointments for the clinics that the current doctor manages
            var appointments = _context.Appointments
                .Include(a => a.Clinic)
                .Include(a => a.Patient)
                .Where(a => a.Clinic.DoctorId == doctorId)
                .OrderBy(a => a.Date)
                .ToList();

            return View(appointments);
        }

        [HttpPost]
        public IActionResult Confirm(int appointmentId)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == appointmentId);
            if (appointment != null && appointment.Status != "confirmed")
            {
                appointment.Status = "confirmed";
                _context.SaveChanges();
            }

            return RedirectToAction("DoctorAppointments");
        }

        [HttpPost]
        public IActionResult Delete(int appointmentId)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == appointmentId);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
            }

            return RedirectToAction("DoctorAppointments");
        }

    }
}
