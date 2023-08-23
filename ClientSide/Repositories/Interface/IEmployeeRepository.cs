using ClientSide.Models;
using ClientSide.ViewModel.Account;
using ClientSide.ViewModel.Employee;
using ClientSide.ViewModel.Response;

namespace ClientSide.Repositories.Interface
{
    public interface IEmployeeRepository : IGeneralRepository<Employee, Guid>
    {
        Task<ResponseViewModel<EmployeeDTO>> GetEmployeeById(Guid Id);
        public Task<ResponseListVM<ListEmployeeVM>> GetAllEmployee();
    }
}