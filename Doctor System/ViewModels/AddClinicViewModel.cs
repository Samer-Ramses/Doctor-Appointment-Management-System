using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctor_System.ViewModels
{
	public class AddClinicViewModel
	{
		[Required]
		public string DoctorId { get; set; }
		[Display(Name = "Email address")]
		[Required(ErrorMessage = "Email address is required")]
		public string EmailAddress { get; set; }

		[Required(ErrorMessage = "The address is required")]
		[StringLength(200)]
		public string Address { get; set; }
		[Required]
		[Column(TypeName = "decimal(18, 2)")]
		public decimal Price { get; set; }
	}
}
