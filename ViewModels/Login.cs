using System.ComponentModel.DataAnnotations;

namespace TanLiWen_220214W_AS_ASSGN2.ViewModels
{
	public class Login
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}
}
