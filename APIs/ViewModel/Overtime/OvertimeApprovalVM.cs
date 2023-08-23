using APIs.Utilities.Enum;

namespace APIs.ViewModel.Overtime
{
    public class OvertimeApprovalVM
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; }
        public DateTime StartOvertime { get; set; }
        public DateTime EndOvertime { get; set; }
        public DateTime SubmitDate { get; set; }
        public string Deskripsi { get; set; }
        public int Paid { get; set; }
        //update to string
        public Status Status { get; set; }
        public Guid Employee_id { get; set; }
    }
}
