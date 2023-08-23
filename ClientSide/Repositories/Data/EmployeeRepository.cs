using ClientSide.Models;
using ClientSide.Repositories.Interface;
using ClientSide.ViewModel.Account;
using ClientSide.ViewModel.Employee;
using ClientSide.ViewModel.Response;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace ClientSide.Repositories.Data
{
    public class EmployeeRepository : GeneralRepository<Employee, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(string request = "Employee/") : base(request)
        {
        }

        public async Task<ResponseViewModel<EmployeeDTO>> GetEmployeeById(Guid Id)
        {
            ResponseViewModel<EmployeeDTO> employeeResponse = null;
            using (var response = await httpClient.GetAsync(_request + Id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                employeeResponse = JsonConvert.DeserializeObject<ResponseViewModel<EmployeeDTO>>(apiResponse);
            }

            return employeeResponse;
        }
        public async Task<ResponseListVM<ListEmployeeVM>> GetAllEmployee()
        {
            ResponseListVM<ListEmployeeVM> entityVM = null;
            using (var response = httpClient.GetAsync(_request + "GetAllMasterEmployee").Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseListVM<ListEmployeeVM>>(apiResponse);
            }
            return entityVM;
        }
    }
}
