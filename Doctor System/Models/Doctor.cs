using Doctor_System.Models;
using System.ComponentModel.DataAnnotations;

namespace Doctor_System.Models
{
	public class Doctor : ApplicationUser
    {
        public string? ImgPath { get; set; }

        // Navigation property for the one-to-many relationship with DoctorSpecialization
        public virtual ICollection<DoctorSpecialization> Specializations { get; set; }
    }
}
