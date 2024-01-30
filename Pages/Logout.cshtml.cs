using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TanLiWen_220214W_AS_ASSGN2.Models;
using TanLiWen_220214W_AS_ASSGN2.ViewModels;
using static TanLiWen_220214W_AS_ASSGN2.Models.AuditLogServices;

namespace TanLiWen_220214W_AS_ASSGN2.Pages
{
    public class LogoutModel : PageModel
    {
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly IAuditLogService _auditLogService;
		public LogoutModel(SignInManager<ApplicationUser> signInManager,
			IAuditLogService auditLogService)
		{
			this.signInManager = signInManager;
			_auditLogService = auditLogService;
		}
		public void OnGet()
        {
        }
		public async Task<IActionResult> OnPostLogoutAsync()
		{
			var user = await signInManager.UserManager.GetUserAsync(User);

			await signInManager.SignOutAsync();

			var userId = user.Id;
			_auditLogService.LogLogout(userId);
			return RedirectToPage("Login");
		}
		public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("Index");
		}
	}
}
