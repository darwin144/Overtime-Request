using ClientSide.Repositories.Interface;
using ClientSide.ViewModel.Response;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide.Repositories
{
    public class GeneralRepository<TEntity, TId> : IGeneralRepository<TEntity, TId>
        where TEntity : class
    {
        public readonly string _request;
        public readonly IHttpContextAccessor _contextAccessor;
        public readonly HttpClient httpClient;

        public GeneralRepository(string request)
        {
            _request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7173/API-Overtimes/")
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _contextAccessor.HttpContext.Session.GetString("JWToken"));
        }

        public async Task<ResponseMessageVM> Deletes(Guid Guid)
        {
            ResponseMessageVM entityVM = null;
            using (var response = httpClient.DeleteAsync(_request + Guid).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseMessageVM>(apiResponse);
            }
            return entityVM;
        }
        public async Task<ResponseViewModel<TEntity>> Get(Guid id)
        {
            ResponseViewModel<TEntity> entity = null;

            using (var response = await httpClient.GetAsync(_request + id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<ResponseViewModel<TEntity>>(apiResponse);
            }
            return entity;
        }

        public async Task<ResponseMessageVM> Put(TEntity entity)
        {
            ResponseMessageVM entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PutAsync(_request, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseMessageVM>(apiResponse);
            }
            return entityVM;
        }


        public async Task<ResponseMessageVM> Post(TEntity entity)
        {
            ResponseMessageVM entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(_request, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseMessageVM>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseListVM<TEntity>> Get()
        {
            ResponseListVM<TEntity> entityVM = null;
            using (var response = await httpClient.GetAsync(_request))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseListVM<TEntity>>(apiResponse);
            }
            return entityVM;
        }

        public IEnumerable<Claim> ExtractClaims(string jwtToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);
            IEnumerable<Claim> claims = securityToken.Claims;
            return claims;
        }
    }
}