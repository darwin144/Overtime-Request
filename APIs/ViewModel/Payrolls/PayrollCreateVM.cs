using System.ComponentModel.DataAnnotations;

namespace APIs.ViewModel.Payrolls
{
    public class PayrollCreateVM
    {
        public Guid Id { get; set; }
        [Required]
        public DateTime PayDate { get; set; }
        [Required]
        public Guid Employee_id { get; set; }
    }
}
