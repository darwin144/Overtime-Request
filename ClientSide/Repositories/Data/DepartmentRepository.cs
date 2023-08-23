using ClientSide.Models;
using ClientSide.Repositories.Interface;
using Newtonsoft.Json;

namespace ClientSide.Repositories.Data
{
    public class DepartmentRepository : GeneralRepository<Department, Guid>, IDepartmentRepository
    {
        private readonly HttpClient httpClient;
        private readonly string request;
        public DepartmentRepository(string request = "Department/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7044/API-Payroll/")
            };
        }
    }
}
