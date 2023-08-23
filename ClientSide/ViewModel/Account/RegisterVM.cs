using System.ComponentModel.DataAnnotations;

namespace ClientSide.ViewModel.Account
{
    public class RegisterVM
    {
        public string? NIK { get; set; }
        [Required(ErrorMessage ="Tidak Boleh Kosong !!")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime HiringDate { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public Guid? ReportTo { get; set; }
        [Required]
        public Guid EmployeeLevel_id { get; set; }
        [Required]
        public Guid Department_id { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}


