namespace APIs.ViewModel.Payrolls
{
    public class PayrollPrintVM
    {
        public Guid Id { get; set; }
        public string PayDate { get; set; }
        public string Fullname { get; set; }
        public string Department { get; set; }
        public string Title { get; set; }
        public int? Allowence { get; set; }
        public int? Overtime { get; set; }
        public int? PayrollCuts { get; set; }
        public int TotalSalary { get; set; }
        public Guid? Employee_id { get; set; }
    }
}
