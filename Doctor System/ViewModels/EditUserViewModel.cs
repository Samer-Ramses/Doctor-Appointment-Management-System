using System.ComponentModel.DataAnnotations;

namespace Doctor_System.ViewModels
{
    public class EditUserViewModel
    {
        [Required(ErrorMessage = "The name is required")]
        public string Name { get; set; }
		[Display(Name = "Phone Number")]
		[Required(ErrorMessage = "The phone number is required")]
		[RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Invalid phone number format. The phone number must start with 010, 011, 012, or 015 and have 11 digits.")]
		public string PhoneNumber { get; set; }
		[Required(ErrorMessage = "The age is required")]
        [Range(1, 120, ErrorMessage = "The age should be in the range 1 to 120")]
        public int Age { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        //[Required(ErrorMessage = "Image is reduired")]
        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }
    }
}
