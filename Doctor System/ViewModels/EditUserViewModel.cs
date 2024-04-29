using System.ComponentModel.DataAnnotations;

namespace Doctor_System.ViewModels
{
    public class EditUserViewModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "The name is required")]
        public string Name { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "The phone number is required")]
        [MaxLength(11, ErrorMessage = "The phone number must be 11 numbers")]
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
