using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TanLiWen_220214W_AS_ASSGN2.ViewModels
{
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Gender { get; set; }
		public string NRIC { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }
		public string ResumeFileName { get; set; }
		public string ResumeFilePath { get; set; }
		public string WhoAmI { get; set; }
	}
}
