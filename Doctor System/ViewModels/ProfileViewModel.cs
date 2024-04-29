using Doctor_System.Models;

namespace Doctor_System.ViewModels
{
	public class ProfileViewModel
	{
		public Doctor? Doctor { get; set; }
		public Patient? Patient { get; set; }
		public List<DoctorSpecialization>? DoctorSpecializations { get; set; } 
	}
}
