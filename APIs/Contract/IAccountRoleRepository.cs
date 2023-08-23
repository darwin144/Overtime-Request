using APIs.Model;
using APIs.ViewModel.AccountRoles;

namespace APIs.Contract
{
    public interface IAccountRoleRepository : IGenericRepository<AccountRole>
    {
        IEnumerable<MasterAccountRoleVM> GetAllMasterAccountRole();
    }
}
