namespace TanLiWen_220214W_AS_ASSGN2.Models
{
	public class AuditLogServices
	{
		public interface IAuditLogService
		{
			void LogLogin(string userId);
			void LogLogout(string userId);
			void LogRegistration(string userId);
		}

		public class AuditLogService : IAuditLogService
		{
			private readonly AuthDbContext _context;

			public AuditLogService(AuthDbContext context)
			{
				_context = context;
			}

			public void LogLogin(string userId)
			{
				LogActivity(userId, "Login");
			}

			public void LogLogout(string userId)
			{
				LogActivity(userId, "Logout");
			}

			public void LogRegistration(string userId)
			{
				LogActivity(userId, "Registration");
			}

			private void LogActivity(string userId, string action)
			{
				var auditLog = new AuditLog
				{
					UserId = userId,
					Action = action,
					Timestamp = DateTime.Now,
				};

				_context.AuditLogs.Add(auditLog);
				_context.SaveChanges();
			}
		}

	}
}
