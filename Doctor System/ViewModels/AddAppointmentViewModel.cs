using System;
using System.ComponentModel.DataAnnotations;

namespace Doctor_System.ViewModels
{
    public class AddAppointmentViewModel
    {
        [Required(ErrorMessage = "Patient name is required")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Appointment date is required")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Appointment time is required")]
        [DataType(DataType.Time)]
        public DateTime AppointmentTime { get; set; }
    }
}
