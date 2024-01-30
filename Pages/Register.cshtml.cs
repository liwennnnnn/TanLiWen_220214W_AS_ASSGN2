using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TanLiWen_220214W_AS_ASSGN2.ViewModels;
using static TanLiWen_220214W_AS_ASSGN2.Models.AuditLogServices;

namespace TanLiWen_220214W_AS_ASSGN2.Pages
{
    // Initialize the build-in ASP.NET Identity
    public class RegisterModel : PageModel
    {
        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }

		private readonly IWebHostEnvironment _environment;

		private readonly IAuditLogService _auditLogService;

		[BindProperty]
        public Register RModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment environment, IAuditLogService auditLogService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
			_environment = environment;
			_auditLogService = auditLogService;
		}

        public void OnGet()
        {
        }

        private async Task AddUserClaimsAsync(ApplicationUser user)
        {
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, user.FirstName));
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, user.LastName));
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Gender, user.Gender));
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, user.Id));
            await userManager.AddClaimAsync(user, new Claim("NRIC", user.NRIC));
            await userManager.AddClaimAsync(user, new Claim("DateOfBirth", user.DateOfBirth.ToString()));
        }

        //Save data into the database
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
				var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
				var protector = dataProtectionProvider.CreateProtector("MySecretKey");

				var user = new ApplicationUser()
                {
                    UserName = RModel.FirstName + RModel.LastName,
					FirstName = RModel.FirstName,
					LastName = RModel.LastName,
					Gender = RModel.Gender,
					NRIC = protector.Protect(RModel.NRIC),
					DateOfBirth = RModel.DateOfBirth?.Date,
					Email = RModel.Email,
					ResumeFileName = RModel.Resume.FileName,
					WhoAmI = RModel.WhoAmI,
				};

				if (RModel.Resume != null && RModel.Resume.Length > 0)
				{
					// Logic to handle file upload
					var fileName = Guid.NewGuid().ToString() + "_" + RModel.Resume.FileName;
					var filePath = Path.Combine(_environment.ContentRootPath, @"wwwroot/uploads", fileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await RModel.Resume.CopyToAsync(stream);
					}

					// Update user's resume information
					user.ResumeFileName = fileName;
					user.ResumeFilePath = filePath;
				}

				// Check for password complexity
				var passwordValidator = userManager.PasswordValidators.First();
                var result = await passwordValidator.ValidateAsync(userManager, user, RModel.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return Page();
                }

                var createResult = await userManager.CreateAsync(user, RModel.Password);

                if (createResult.Succeeded)
                {
                    await AddUserClaimsAsync(user);

                    await signInManager.SignInAsync(user, false);
					var userId = user.Id;
					_auditLogService.LogRegistration(userId);
					return RedirectToPage("Index");
                }

                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }
    }
}
