using APIs.Context;
using APIs.Contract;
using APIs.Model;

namespace APIs.Repositories
{
    public class RoleRepository : GeneralRepository<Role>, IRoleRepository
    {
        public RoleRepository(PayrollOvertimeContext context) : base(context)
        {
        }
    }
}
