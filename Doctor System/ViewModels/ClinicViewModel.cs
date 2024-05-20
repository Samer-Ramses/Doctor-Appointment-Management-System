using Doctor_System.Models;

namespace Doctor_System.ViewModels
{
	public class ClinicViewModel
	{
		public Clinic Clinic { get; set; }
		public Doctor Doctor { get; set; }
		public ICollection<DoctorSpecialization>? DoctorSpecializations { get; set; }
		public ICollection<ClinicWorkingHours> ClinicWorkingHours { get; set; }
	}
}
