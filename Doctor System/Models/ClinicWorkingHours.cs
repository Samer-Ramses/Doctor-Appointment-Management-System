using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Doctor_System.Models
{
	public class ClinicWorkingHours
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("Clinic")]
		public int ClinicId { get; set; }

		[Required]
		public string DayOfWeek { get; set; }

		public TimeSpan OpeningTime { get; set; }
		public TimeSpan ClosingTime { get; set; }

		// Navigation property for the many-to-one relationship with Clinic
		public virtual Clinic Clinic { get; set; }
	}
}
