using ClientSide.Utilities;

namespace ClientSide.ViewModel.Overtime
{
    public class OvertimeDetailVM
    {
        public string? FullName { get; set; }
        public DateTime StartOvertime { get; set; }
        public DateTime EndOvertime { get; set; }
        public string SubmitDate { get; set; }
        public string Deskripsi { get; set; }
        public Status? Status { get; set; }
    }
}
