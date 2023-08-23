using ClientSide.Models;
using ClientSide.ViewModel.Overtime;
using ClientSide.ViewModel.Response;

namespace ClientSide.Repositories.Interface
{
    public interface IOvertimeRepository : IGeneralRepository<Overtime, Guid>
    {
        Task<ResponseMessageVM> RequestOvertime(Overtime overtime);
        Task<IEnumerable<OvertimeDetailVM>> GetOvertimeByemployeeGuid(Guid id);
        Task<int> ApprovalOvertime();
    }
}
