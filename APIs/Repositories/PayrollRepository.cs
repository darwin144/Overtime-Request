using APIs.Context;
using APIs.Contract;
using APIs.Model;
using APIs.Utilities.Enum;
using APIs.ViewModel.Payrolls;
using System.Globalization;

namespace APIs.Repositories
{
    public class PayrollRepository : GeneralRepository<Payroll>, IPayrollRepository
    {
        public PayrollRepository(PayrollOvertimeContext context) : base(context)
        {
        }

        public Payroll CreatePayroll(PayrollCreateVM payrollCreate)
        {
            try
            {
                var today = DateTime.Today;
                var targetDate = new DateTime(today.Year, today.Month, 25);
                var endDate = targetDate.AddDays(-30);

                if (payrollCreate.PayDate.Day == targetDate.Day)
                {
                    var employeeLevel = _context.Employees.Where(a => a.Id == payrollCreate.Employee_id)
                                .Join(_context.EmployeeLevels, e => e.EmployeeLevel_id, el => el.Id, (e, el) => new { e.EmployeeLevel, el.Employees })
                                .FirstOrDefault();

                    var allowence = employeeLevel.EmployeeLevel.Allowence;
                    var salary = employeeLevel.EmployeeLevel.Salary;
                    var overtime = _context.Overtimes.Where(a => a.Employee_id == payrollCreate.Employee_id && a.SubmitDate.Day <= targetDate.Day && a.SubmitDate.Day >= endDate.Day && a.Status == Status.Approved)
                                    .Select(a => a.Paid).Sum();
                    
                    var pph = 0.0042 * salary;
                    var bpjs = (salary + allowence) * 0.01;
                    var totalCuts = pph + bpjs;
                    var totalSalary = salary + overtime + allowence - totalCuts;

                    var model = new Payroll
                    {
                        Id = payrollCreate.Id,
                        PayDate = payrollCreate.PayDate,
                        PayrollCuts = Convert.ToInt32(totalCuts),
                        TotalSalary = Convert.ToInt32(totalSalary),
                        Employee_id = payrollCreate.Employee_id
                    };

                    _context.Set<Payroll>().Add(model);
                    _context.SaveChanges();
                    return model;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<PayrollPrintVM> GetAllDetailPayrolls()
        {
            try
            {
                var today = DateTime.Today;
                var targetDate = new DateTime(today.Year, today.Month, 25);

                var modelsVM = GetAll()
                                .Join(_context.Employees, p => p.Employee_id, e => e.Id, (p, e) => new { p, e })
                                .Join(_context.EmployeeLevels, pe => pe.e.EmployeeLevel_id, el => el.Id, (pe, el) => new { pe, el })
                                .Join(_context.Overtimes, pel => pel.pe.e.Id, o => o.Employee_id, (pel, o) => new { pel, o })
                                .Join(_context.Departments, pelo => pelo.pel.pe.e.Department_id, d => d.Id, (pelo, d) => new PayrollPrintVM
                                {
                                    Id = pelo.pel.pe.p.Id,
                                    PayDate = pelo.pel.pe.p.PayDate.ToString("dd MMMM yyyy", new CultureInfo("id-ID")),
                                    Fullname = pelo.pel.pe.e.FirstName + " " + pelo.pel.pe.e.LastName,
                                    Department = d.Name,
                                    Title = pelo.pel.el.Title,
                                    Allowence = pelo.pel.el.Allowence,
                                    Overtime = _context.Overtimes.Where(a => a.Employee_id == pelo.pel.pe.e.Id && a.SubmitDate.Day <= targetDate.Day && a.Status == Status.Approved)
                                                         .Select(a => a.Paid).Sum(),
                                    PayrollCuts = pelo.pel.pe.p.PayrollCuts,
                                    TotalSalary = pelo.pel.pe.p.TotalSalary,
                                    Employee_id = pelo.pel.pe.e.Id
                                }).Distinct()
                                .ToList();
                return modelsVM;

            }
            catch {
                return null;
            }
        }
        public IEnumerable<PayrollPrintVM> GetAllDetailPayrollsByEmployeeID(Guid id)
        {
            try
            {
                var listPayrollEmployee = new List<PayrollPrintVM>();
                var listPayroll = GetAllDetailPayrolls();
                foreach (var item in listPayroll)
                {
                    if (item.Employee_id == id)
                    {
                        listPayrollEmployee.Add(item);
                    }
                }
                return listPayrollEmployee;

            }
            catch {
                return null;
            }
        }
    }
}