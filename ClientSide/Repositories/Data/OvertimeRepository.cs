using ClientSide.Models;
using ClientSide.Repositories.Interface;
using ClientSide.ViewModel.Overtime;
using ClientSide.ViewModel.Response;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;
using System.Text;

namespace ClientSide.Repositories.Data
{
    public class OvertimeRepository : GeneralRepository<Overtime, Guid>, IOvertimeRepository
    {
        public OvertimeRepository(string request = "Overtime/") : base(request)
        {
        }
        public Task<int> ApprovalOvertime()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OvertimeDetailVM>> GetOvertimeByemployeeGuid(Guid id)
        {
            ResponseListVM<Overtime> overtimes = null;

            using (var response = await httpClient.GetAsync(_request + "ByEmployee/" + id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                overtimes = JsonConvert.DeserializeObject<ResponseListVM<Overtime>>(apiResponse);
            }

            var employeeOvertimes = new List<OvertimeDetailVM>();

            if (overtimes != null)
            {
                employeeOvertimes = overtimes.Data?.Select(e => new OvertimeDetailVM
                {
                    StartOvertime = e.StartOvertime,
                    EndOvertime = e.EndOvertime,
                    SubmitDate = e.SubmitDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("id-ID")),
                    Deskripsi = e.Deskripsi,
                    Status = e.Status
                }).ToList();
            }

            return employeeOvertimes;
        }
        public async Task<ResponseMessageVM> RequestOvertime(Overtime overtime)
        {
            ResponseMessageVM entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(overtime), Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await httpClient.PostAsync(_request + "OvertimeRequest/", content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseMessageVM>(apiResponse);
            }
            return entityVM;
        }
    }
}
