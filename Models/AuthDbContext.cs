using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TanLiWen_220214W_AS_ASSGN2.Models;
using TanLiWen_220214W_AS_ASSGN2.ViewModels;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
	private readonly IConfiguration _configuration;

	public AuthDbContext(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string connectionString = _configuration.GetConnectionString("AuthConnectionString");
		optionsBuilder.UseSqlServer(connectionString);
	}

	public DbSet<AuditLog> AuditLogs { get; set; }
}
