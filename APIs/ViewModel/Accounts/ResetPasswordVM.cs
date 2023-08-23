using System.ComponentModel.DataAnnotations;

namespace APIs.ViewModel.Accounts
{
    public class ResetPasswordVM
    {
        public int OTP { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
