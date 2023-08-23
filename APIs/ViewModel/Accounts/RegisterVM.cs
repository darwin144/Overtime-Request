using APIs.Utilities;
using System.ComponentModel.DataAnnotations;

namespace APIs.ViewModel.Accounts
{
    public class RegisterVM
    {
        public string? NIK { get; set; }

        [Required(ErrorMessage = "First Name is Required ")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime HiringDate { get; set; }

        [EmailAddress]
        [Validation(nameof(Email))]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public Guid? ReportTo { get; set; }

        [Required]
        public Guid EmployeeLevel_id { get; set; }

        [Required]
        public Guid Department_id { get; set; }

        [Required]
        [PasswordValidation(ErrorMessage = "Minimal 6 character, 1 uppercase, 1 lower case, 1 Symbol, 1 number")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
