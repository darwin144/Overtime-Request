using APIs.Model;
using APIs.ViewModel.Employees;

namespace APIs.Contract
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        bool CheckValidation(string value);
        Employee FindEmployeeByEmail(string email);
        int CreateWithValidate(Employee emloyee);
        IEnumerable<MasterEmployeeVM> GetAllMasterEmployee();
    }
}
