using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doctor_System.Models;
using Doctor_System.Data;

namespace Doctor_System.Controllers
{
    [Route("[controller]")]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointment/Index
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var appointments = _context.Appointments.Include(a => a.Clinic).Include(a => a.Patient).ToList();
            return View(appointments);
        }

        // GET: Appointment/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            // You might want to populate a dropdown for clinics or patients here
            return View("Index");
        }

        // POST: Appointment/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                _context.SaveChanges();
                return RedirectToAction("Index", "Doctor");     // توجيه المستخدم إلى Index بعد إضافة الموعد
            }
            return View(appointment);
        }

        // GET: Appointment/Edit/5
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = _context.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        // POST: Appointment/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        // GET: Appointment/Delete/5
        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = _context.Appointments.FirstOrDefault(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var appointment = _context.Appointments.Find(id);
            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
