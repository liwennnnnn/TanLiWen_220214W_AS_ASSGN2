using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TanLiWen_220214W_AS_ASSGN2.ViewModels;
using static TanLiWen_220214W_AS_ASSGN2.Models.AuditLogServices;
using Newtonsoft.Json;

namespace TanLiWen_220214W_AS_ASSGN2.Pages
{
    public class LoginModel : PageModel
    {
		[BindProperty]
		public Login LModel { get; set; }

		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly ILogger<LoginModel> logger;
		private readonly IAuditLogService _auditLogService;
		public LoginModel(SignInManager<ApplicationUser> signInManager,
			ILogger<LoginModel> logger,
			 IAuditLogService auditLogService)
		{
			this.signInManager = signInManager;
			this.logger = logger;
			_auditLogService = auditLogService;
		}
		public void OnGet()
        {
        }
		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var user = await signInManager.UserManager.FindByEmailAsync(LModel.Email);

				if (user != null)
				{
					// Verify reCAPTCHA
					var recaptchaResponse = Request.Form["g-recaptcha-response"];
					var recaptchaSecretKey = "6LfJQmApAAAAADdBt1qyMcnXp74CpKARDld30XO5";

					var httpClient = new HttpClient();
					var response = await httpClient.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={recaptchaSecretKey}&response={recaptchaResponse}");

					logger.LogInformation($"reCAPTCHA API Response: {response}");

					var recaptchaResult = JsonConvert.DeserializeObject<RecaptchaResponse>(response);

					if (!recaptchaResult.Success)
					{
						ModelState.AddModelError("", "reCAPTCHA validation failed. Please try again.");

						if (recaptchaResult.ErrorCodes != null)
						{
							foreach (var errorCode in recaptchaResult.ErrorCodes)
							{
								ModelState.AddModelError("", $"reCAPTCHA Error: {errorCode}");
							}
						}

						return Page();
					}
					var result = await signInManager.PasswordSignInAsync(user, LModel.Password, LModel.RememberMe, lockoutOnFailure: true);

					if (result.Succeeded)
					{
						var claims = new List<Claim>
						{
							new Claim(ClaimTypes.Email, LModel.Email)
						};

						var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
						var authProperties = new AuthenticationProperties
						{
							AllowRefresh = true,
							IsPersistent = true, // Remember the user across sessions
							ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(20)), // Set the expiration time
						};

						await HttpContext.SignInAsync(
							CookieAuthenticationDefaults.AuthenticationScheme,
							new ClaimsPrincipal(claimsIdentity),
							authProperties);

						var userId = user.Id;

						_auditLogService.LogLogin(userId);

						return RedirectToPage("Index");
					}
					else
					{
						if (result.IsLockedOut)
						{
							// Handle locked out scenario
							ModelState.AddModelError("", "Account locked out. Please try again later.");
						}
						else
						{
							// Log the error
							logger.LogError($"Login failed: {LModel.Email}");
							logger.LogError($"Login result: {result}");
							ModelState.AddModelError("", "Email or Password incorrect");
						}
					}
				}
				else
				{
					// Log the error - user not found
					logger.LogError($"User not found for email: {LModel.Email}");
					ModelState.AddModelError("", "Please register for your account");
				}
			}

			return Page();
		}


	}

	public class RecaptchaResponse
	{
		public bool Success { get; set; }
		public List<string> ErrorCodes { get; set; }
	}

}
