using APIs.Model;
using APIs.ViewModel.Overtime;

namespace APIs.Contract
{
    public interface IOvertimeRepository : IGenericRepository<Overtime>
    {
        Overtime CreateRequest(Overtime overtime);
        int ApprovalOvertime(Overtime overtime, Guid id);
        List<OvertimeApprovalVM> ListOvertimeByIdManager(Guid managerId);

        IEnumerable<OvertimeVM> ListOvertimeByIdEmployee(Guid idEmployee);

        IEnumerable<OvertimeRemainingVM> ListRemainingOvertime();
        IEnumerable<OvertimeRemainingVM> ListRemainingOvertimeByGuid(Guid id);
        OvertimeRemainingVM RemainingOvertimeByEmployeeGuid(Guid id);
        ChartVM DataChartByGuid(Guid id);
    }
}
