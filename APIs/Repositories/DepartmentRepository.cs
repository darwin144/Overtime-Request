using APIs.Context;
using APIs.Contract;
using APIs.Model;

namespace APIs.Repositories
{
    public class DepartmentRepository : GeneralRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(PayrollOvertimeContext context) : base(context)
        {
        }
    }
}
