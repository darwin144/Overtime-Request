namespace ClientSide.ViewModel.Payroll
{
    public class PayrollCreateVM
    {
        public Guid Id { get; set; }

        public DateTime PayDate { get; set; }

        public Guid Employee_id { get; set; }
    }
}
