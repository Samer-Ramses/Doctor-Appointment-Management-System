using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Doctor_System.Models
{
	public class DoctorSpecialization
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("Doctor")]
		public string DoctorId { get; set; }

		[Required]
		[StringLength(100)]
		public string Specialization { get; set; }

		// Navigation property for the many-to-one relationship with Doctor
		public virtual Doctor Doctor { get; set; }
	}
}
