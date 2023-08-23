namespace APIs.ViewModel.Employees
{
    public class MasterEmployeeVM
    {
        public Guid Id { get; set; }
        public string NIK { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public DateTime HiringDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ReportTo { get; set; }
        public string Title { get; set; }
        public string DepartmentName { get; set; }
    }
}
