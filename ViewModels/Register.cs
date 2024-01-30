using System.ComponentModel.DataAnnotations;


namespace TanLiWen_220214W_AS_ASSGN2.ViewModels
{
    public class Register
    {
        [Required(ErrorMessage = "Please enter your first name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please select your gender")]
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please enter your NRIC")]
        [DataType(DataType.Text)]
        [RegularExpression("^[STFG]\\d{7}[A-Z]$", ErrorMessage = "Invalid NRIC format")]
        public string NRIC { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Date of Birth")]
		[Required(ErrorMessage = "Please enter your date of birth")]
		[Range(typeof(DateTime), "1950-01-01", "2010-01-01", ErrorMessage = "Invalid date of birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateOfBirth { get; set; }

		[Required(ErrorMessage = "Please enter your email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters long")]
        [PasswordComplexity(ErrorMessage = "Password must include at least one lower-case letter, one upper-case letter, one number, and one special character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Resume")]
		[Required(ErrorMessage = "Please upload your resume")]
		[AllowedExtensions(new string[] { ".docx", ".pdf" }, ErrorMessage = "Invalid file format. Only .docx or .pdf allowed.")]
        public IFormFile Resume { get; set; }

        [Required(ErrorMessage = "Please provide some information about yourself")]
        [DataType(DataType.MultilineText)]
        public string WhoAmI { get; set; }
    }

    // Check password complexity
    public class PasswordComplexityAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string password = value as string;

            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            // Check for minimum length
            if (password.Length < 12)
            {
                return false;
            }

            // Check for at least one lower-case letter
            if (!password.Any(char.IsLower))
            {
                return false;
            }

            // Check for at least one upper-case letter
            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            // Check for at least one digit
            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            // Check for at least one special character
            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                return false;
            }

            return true;
        }
    }

    // Check file uploaded
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

		public AllowedExtensionsAttribute(string[] extensions)
		{
			_extensions = extensions;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is IFormFile file)
			{
				var fileName = Path.GetFileName(file.FileName);
				var fileExtension = Path.GetExtension(fileName);

				if (!_extensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
				{
					var allowedExtensions = string.Join(", ", _extensions);
					return new ValidationResult($"Only {allowedExtensions} file types are allowed");
				}
			}

			return ValidationResult.Success;
		}
    }
}
