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
	[Authorize(Roles = "Patient")]
	public class PatientController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ApplicationDbContext _context;

		public PatientController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_context = context;
		}
		public IActionResult AllClinics()
		{
			var clinics = _context.Clinics.ToList();
			List<ClinicViewModel> clinicList = new();
            foreach (var item in clinics)
            {
				var doctor = _context.Doctors.FirstOrDefault(doc => doc.Id == item.DoctorId);
				var WorkingHours = _context.ClinicsWorkingHours.Where(cwh => cwh.ClinicId == item.Id).ToList();
				var doctorSpecs = _context.DoctorsSpecializations.Where(ds => ds.DoctorId == item.DoctorId).ToList();
				var clinic = new ClinicViewModel
				{
					Clinic = item,
					Doctor = doctor,
					ClinicWorkingHours = WorkingHours,
					DoctorSpecializations = doctorSpecs
				};

				clinicList.Add(clinic);
            }
            return View(clinicList);
		}

        public IActionResult MakeAppointment(int clinicId)
        {
            var clinic = _context.Clinics.Include(c => c.WorkingHours).FirstOrDefault(c => c.Id == clinicId);
            if (clinic == null)
            {
                return NotFound();
            }

            var today = DateTime.Today.AddDays(1);
            var upcomingWeek = today.AddDays(7);
            var appointments = _context.Appointments
                .Where(a => a.ClinicId == clinicId && a.Date >= today && a.Date <= upcomingWeek)
                .Select(a => a.Date)
                .ToList();

            List<AppointmentSlotViewModel> appointmentSlots = new();

            for (var date = today; date <= upcomingWeek; date = date.AddDays(1))
            {
                var dayOfWeek = date.DayOfWeek.ToString();
                var workingHours = clinic.WorkingHours.FirstOrDefault(wh => wh.DayOfWeek.ToLower() == dayOfWeek.ToLower());

                if (workingHours != null)
                {
                    var startTime = date.Add(workingHours.OpeningTime);
                    var endTime = date.Add(workingHours.ClosingTime);
                    for (var time = startTime; time < endTime; time = time.AddMinutes(30))
                    {
                        if (!appointments.Contains(time))
                        {
                            appointmentSlots.Add(new AppointmentSlotViewModel
                            {
                                ClinicId = clinicId,
                                Date = time
                            });
                        }
                    }
                }
            }

            return View(appointmentSlots);
        }

        [HttpPost]
        public IActionResult Book(int clinicId, DateTime date)
        {
            // Assuming you have the current user's ID from your authentication system
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var appointment = new Appointment
            {
                ClinicId = clinicId,
                PatientId = userId,
                Date = date,
                Status = "processing"
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return RedirectToAction("YourAppointments");
        }

        public IActionResult YourAppointments()
        {
            // Assuming you have the current user's ID from your authentication system
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch appointments for the current user
            var appointments = _context.Appointments
                .Include(a => a.Clinic)
                .Where(a => a.PatientId == userId)
                .OrderBy(a => a.Date)
                .ToList();

            return View(appointments);
        }

        [HttpPost]
        public IActionResult Cancel(int appointmentId)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == appointmentId);
            if (appointment != null)
            {
                if (appointment.Status != "confirmed")
                {
                    _context.Remove(appointment);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("YourAppointments");
        }
    }
}
