
using Doctor_System.Data;
using System.ComponentModel.DataAnnotations;

namespace Doctor_System.ViewModels
{
    public class RegisterViewModel : IValidatableObject
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "The name is required")]
        public string Name { get; set; }
		[Display(Name = "Phone Number")]
		[Required(ErrorMessage = "The phone number is required")]
		[RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Invalid phone number format. The phone number must start with 010, 011, 012, or 015 and have 11 digits.")]
		public string PhoneNumber { get; set; }
		[Required(ErrorMessage = "The age is required")]
        [Range(1, 120, ErrorMessage = "The age should be in the range 1 to 120")]
        public int Age { get; set; }
        [Required(ErrorMessage = "The role is required")]
        public string Role { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
        //[Required(ErrorMessage = "Image is reduired")]
        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (Role == Roles.Doctor && Age <= 26)
            {
                yield return new ValidationResult("Doctor age should be greater than 26", new[] { nameof(Role), nameof(Age) });
            }
        }
    }
}
