using APIs.Context;
using APIs.Contract;
using APIs.Model;
using APIs.ViewModel.Employees;
using Microsoft.EntityFrameworkCore;

namespace APIs.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(PayrollOvertimeContext context) : base(context)
        {
        }
        public bool CheckValidation(string value)
        {
            return _context.Employees
            .Any(e => e.Email == value || e.PhoneNumber == value || e.NIK == value);
        }
        public Employee FindEmployeeByEmail(string email)
        {

            return _context.Set<Employee>().FirstOrDefault(a => a.Email == email);
        }

        public int CreateWithValidate(Employee employee)
        {
            try
            {
                bool ExistsByEmail = _context.Employees.Any(e => e.Email == employee.Email);
                if (ExistsByEmail)
                {
                    return 1;
                }

                bool ExistsByPhoneNumber = _context.Employees.Any(e => e.PhoneNumber == employee.PhoneNumber);
                if (ExistsByPhoneNumber)
                {
                    return 2;
                }

                Create(employee);
                return 3;
            }
            catch
            {
                return 0;
            }
        }

        public IEnumerable<MasterEmployeeVM> GetAllMasterEmployee()
        {
            var employees = GetAll();
            var empLevels = _context.EmployeeLevels.ToList();
            var departments = _context.Departments.ToList();

            var allEmployees = new List<MasterEmployeeVM>();
            foreach (var employee in employees)
            {
                var empLevel = empLevels.FirstOrDefault(el => el.Id == employee.EmployeeLevel_id);
                var department = departments.FirstOrDefault(d => d.Id == employee?.Department_id);
                var reportToFullName = string.Empty;

                if (empLevel != null && department != null)
                {
                    if (employee.ReportTo.HasValue)
                    {
                        var reportToEmployee = employees.FirstOrDefault(e => e.Id == employee.ReportTo.Value);
                        if (reportToEmployee != null)
                        {
                            reportToFullName = reportToEmployee.FirstName + " " + reportToEmployee.LastName;
                        }
                    }
                    var allEmployee = new MasterEmployeeVM
                    {

                        Id = employee.Id,
                        NIK = employee.NIK,
                        FullName = employee.FirstName + " " + employee.LastName,
                        BirthDate = employee.BirthDate,
                        Gender = employee.Gender,
                        HiringDate = employee.HiringDate,
                        Email = employee.Email,
                        PhoneNumber = employee.PhoneNumber,
                        ReportTo = reportToFullName,
                        Title = empLevel.Title,
                        DepartmentName = department.Name,


                    };
                    allEmployees.Add(allEmployee);
                }

            }
            return allEmployees;
        }
    }
}
