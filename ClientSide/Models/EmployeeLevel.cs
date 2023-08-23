namespace ClientSide.Models
{
    public class EmployeeLevel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Level { get; set; }
        public int Salary { get; set; }
        public int? Allowence { get; set; }
    }
}
