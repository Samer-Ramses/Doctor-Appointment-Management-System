using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctor_System.ViewModels
{
	public class AddClinicViewModel
	{
		[Required]
		public string DoctorId { get; set; }
		[Display(Name = "Phone Number")]
		[Required(ErrorMessage = "The phone number is required")]
		[RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Invalid phone number format. The phone number must start with 010, 011, 012, or 015 and have 11 digits.")]
		public string PhoneNumber { get; set; }
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
