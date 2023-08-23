
namespace ClientSide.Models
{
    public class Payroll
    {

        public Guid Id { get; set; }
        public DateTime PayDate { get; set; }
        public int PayrollCuts { get; set; }
        public int TotalSalary { get; set; }
        public Guid Employee_id { get; set; }
    }
}
