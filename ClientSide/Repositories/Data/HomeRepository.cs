using ClientSide.Models;
using ClientSide.ViewModel.Account;
using ClientSide.ViewModel.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide.Repositories.Data
{
    public class HomeRepository : GeneralRepository<Account, int>
    {
        private readonly HttpClient httpClient;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;

        public HomeRepository(string request = "") : base(request)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7173/API-Overtimes/")
            };
            this.request = request;

        }

        public async Task<ResponseViewModel<string>> Logins(LoginVM login)
        {
            ResponseViewModel<string> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request + "Account/Login", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseViewModel<string>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseMessageVM> Registers(RegisterVM entity)
        {
            ResponseMessageVM entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request + "Account/Register", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseMessageVM>(apiResponse);
            }
            return entityVM;
        }

    }
}
