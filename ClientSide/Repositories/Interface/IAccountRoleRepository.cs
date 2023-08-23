using ClientSide.Models;
using ClientSide.ViewModel.Account;
using ClientSide.ViewModel.Response;

namespace ClientSide.Repositories.Interface
{
    public interface IAccountRoleRepository : IGeneralRepository<AccountRole, Guid>
    {
        public Task<ResponseListVM<ListAccountRoleVM>> GetAllMasterAccountRole();
    }
}
