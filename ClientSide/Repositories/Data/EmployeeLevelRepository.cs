
using ClientSide.Models;
using ClientSide.Repositories.Interface;

namespace ClientSide.Repositories.Data
{
    public class EmployeeLevelRepository : GeneralRepository<EmployeeLevel, Guid>, IEmployeeLevelRepository
    {
        private readonly HttpClient httpClient;
        private readonly string request;
        public EmployeeLevelRepository(string request = "EmployeeLevel/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7044/API-Overtimes/")
            };
        }
    }
}
