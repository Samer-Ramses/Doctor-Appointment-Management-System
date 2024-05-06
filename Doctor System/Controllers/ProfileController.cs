using Doctor_System.Data;
using Doctor_System.Models;
using Doctor_System.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Doctor_System.Controllers
{
	public class ProfileController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
			var current = await _userManager.GetUserAsync(User);
			if (current == null)
			{
				return RedirectToAction("Index", "Home");
			}
			if(await _userManager.IsInRoleAsync(current, "Doctor"))
            {
				var currentUser = _context.Doctors.FirstOrDefault(x => x.Id == current.Id);
				ProfileViewModel profileViewModel = new()
				{
					Doctor = currentUser,
					DoctorSpecializations = _context.DoctorsSpecializations.Where(ds => ds.DoctorId == currentUser.Id).ToList(),
				};
                return View(profileViewModel);
			}
			else
            {
				var currentUser = _context.Patients.FirstOrDefault(x => x.Id == current.Id);
				ProfileViewModel profileViewModel = new()
				{
					Patient = currentUser
				};
                return View(profileViewModel);
			}
		}
		public async Task<IActionResult> Settings()
		{
			var currentUser = await _userManager.GetUserAsync(User);
			if (currentUser == null)
			{
				return RedirectToAction("Index", "Home");
			}
            EditUserViewModel registerViewModel = new()
            {
				Age = currentUser.Age,
				EmailAddress = currentUser.Email,
				Name = currentUser.Name,
				PhoneNumber = currentUser.PhoneNumber,
			};

			return View(registerViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Settings(EditUserViewModel editUserViewModel)
		{
			if (!ModelState.IsValid) return View(editUserViewModel);
			var current = await _userManager.GetUserAsync(User);
			if (current == null)
			{
				return RedirectToAction("Index", "Home");
			}
			var currentUser = _context.Doctors.FirstOrDefault(x => x.Id == current.Id);
			if (currentUser != null)
			{
				string filePath = currentUser.ImgPath == null ? "" : currentUser.ImgPath;
				// Process profile picture
				if (editUserViewModel.ProfilePicture != null && editUserViewModel.ProfilePicture.Length > 0)
				{
					// Check if the uploaded file is an image
					if (!IsImageFile(editUserViewModel.ProfilePicture))
					{
						ModelState.AddModelError("ProfilePicture", "The file is not an image.");
						return View(editUserViewModel);
					}

					// Delete old image if exists
					if (!string.IsNullOrEmpty(currentUser.ImgPath))
					{
						string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, currentUser.ImgPath.TrimStart('/'));
						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}

					string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
					string uniqueFileName = Guid.NewGuid().ToString() + "_" + editUserViewModel.ProfilePicture.FileName;
					filePath = Path.Combine(uploadsFolder, uniqueFileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await editUserViewModel.ProfilePicture.CopyToAsync(fileStream);
					}
					currentUser.ImgPath = "/images/" + uniqueFileName;
				}
				currentUser.Email = editUserViewModel.EmailAddress;
				currentUser.UserName = editUserViewModel.EmailAddress;
				currentUser.Name = editUserViewModel.Name;
				currentUser.Age = editUserViewModel.Age;
				currentUser.PhoneNumber = editUserViewModel.PhoneNumber;

				var editedUserResponse = await _userManager.UpdateAsync(currentUser);
			}
			else
			{
				current.Email = editUserViewModel.EmailAddress;
				current.UserName = editUserViewModel.EmailAddress;
				current.Name = editUserViewModel.Name;
				current.Age = editUserViewModel.Age;
				current.PhoneNumber = editUserViewModel.PhoneNumber;

				var editedUserResponse = await _userManager.UpdateAsync(current);
			}


			return RedirectToAction("Index", "Home");
		}

		// Method to check if the uploaded file is an image
		private bool IsImageFile(IFormFile file)
		{
			if (file == null) return false;
			return file.ContentType.ToLower().StartsWith("image/");
		}
	}
}
