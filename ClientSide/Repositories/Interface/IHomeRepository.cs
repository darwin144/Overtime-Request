using ClientSide.Models;
using ClientSide.ViewModel.Account;
using ClientSide.ViewModel.Response;

namespace ClientSide.Repositories.Interface
{
    public interface IHomeRepository : IGeneralRepository<Account, string>
    {
        public Task<ResponseViewModel<string>> Logins(LoginVM entity);
        public Task<ResponseMessageVM> Registers(RegisterVM entity);
        
    }
}