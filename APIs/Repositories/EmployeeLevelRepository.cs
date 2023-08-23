using APIs.Context;
using APIs.Contract;
using APIs.Model;

namespace APIs.Repositories
{
    public class EmployeeLevelRepository : GeneralRepository<EmployeeLevel>, IEmployeeLevelRepository
    {
        public EmployeeLevelRepository(PayrollOvertimeContext context) : base(context)
        {
        }
    }
}