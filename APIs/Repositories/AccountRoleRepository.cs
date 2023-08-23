using APIs.Context;
using APIs.Contract;
using APIs.Model;
using APIs.ViewModel.AccountRoles;

namespace APIs.Repositories
{
    //PERLU DI REFACTOR
    public class AccountRoleRepository : GeneralRepository<AccountRole>, IAccountRoleRepository
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AccountRoleRepository(IAccountRepository accountRepository,IEmployeeRepository employeeRepository, 
                                    PayrollOvertimeContext context) : base(context)
        {
            _accountRepository = accountRepository;
            _employeeRepository = employeeRepository;

        }

        public IEnumerable<MasterAccountRoleVM> GetAllMasterAccountRole()
        {
            try
            {
                var accountRoles = GetAll();
                var roles = _context.Roles.ToList();
                var accounts = _accountRepository.GetAll();
                var employee = _employeeRepository.GetAll();
                var query = from acc in accounts
                            join emp in employee
                            on acc.Employee_id equals emp.Id
                            join accRole in accountRoles
                            on acc.Id equals accRole.Account_id
                            join rl in roles
                            on accRole.Role_id equals rl.Id
                            select new MasterAccountRoleVM
                            {
                                Id = accRole.Id,
                                FirstName = emp.FirstName,
                                LastName = emp.LastName,
                                Email = emp.Email,
                                Name = rl.Name,
                                employee_id = emp.Id
                            };
                return query;
            }
            catch {
                return null;
            }
        }
    }
}
