using ClientSide.Models;
using ClientSide.ViewModel.Account;
using ClientSide.ViewModel.Response;

namespace ClientSide.Repositories.Interface
{
    public interface IManagerRepository : IGeneralRepository<Employee, Guid>
    {
        Task<ResponseViewModel<EmployeeDTO>> GetEmployeeById(Guid Id);
    }
}


