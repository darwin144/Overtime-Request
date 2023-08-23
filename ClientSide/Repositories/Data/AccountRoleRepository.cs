using ClientSide.Models;
using ClientSide.Repositories.Interface;
using ClientSide.ViewModel.Account;
using ClientSide.ViewModel.Response;
using Newtonsoft.Json;

namespace ClientSide.Repositories.Data
{
    public class AccountRoleRepository : GeneralRepository<AccountRole, Guid>, IAccountRoleRepository
    {
        private readonly HttpClient httpClient;
        private readonly string request;
        public AccountRoleRepository(string request = "AccountRole/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7044/API-Overtimes/")
            };
        }

        public async Task<ResponseListVM<ListAccountRoleVM>> GetAllMasterAccountRole()
        {
            ResponseListVM<ListAccountRoleVM> entityVM = null;
            using (var response = httpClient.GetAsync(request + "GetAllMasterAccountRole").Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseListVM<ListAccountRoleVM>>(apiResponse);
            }
            return entityVM;
        }
    }
}
