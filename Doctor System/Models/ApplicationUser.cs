using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Doctor_System.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		[Required]
		[Range(0, 150)]
		public int Age { get; set; }
	}
}
