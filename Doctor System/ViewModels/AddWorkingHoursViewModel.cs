using System.ComponentModel.DataAnnotations;

namespace Doctor_System.ViewModels
{
	public class AddWorkingHoursViewModel : IValidatableObject
	{
		[Display(Name = "Day Of Week")]
		[Required(ErrorMessage = "Day of week is required")]
		public string DayOfWeek { get; set; }
		[Display(Name = "Opening Time")]
		[Required(ErrorMessage = "Opening time is required")]
		public TimeSpan OpeningTime { get; set; }
		[Display(Name = "Closing Time")]
		[Required(ErrorMessage = "Closing Time is required")]
		public TimeSpan ClosingTime { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{

			if (ClosingTime <= OpeningTime)
			{
				yield return new ValidationResult("End time must be greater than start time.", new[] { nameof(OpeningTime), nameof(ClosingTime) });
			}
		}
	}
}
