using Doctor_System.Models;
using System.ComponentModel.DataAnnotations;

namespace Doctor_System.Models
{
	public class Patient : ApplicationUser
    {
        // Navigation property for the one-to-many relationship with Appointment
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
