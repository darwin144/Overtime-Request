using APIs.Context;
using APIs.Contract;
using APIs.Model;
using APIs.Utilities.Enum;
using APIs.ViewModel.Overtime;
using Microsoft.EntityFrameworkCore;

namespace APIs.Repositories
{
    public class OvertimeRepository : GeneralRepository<Overtime>, IOvertimeRepository
    {

        public OvertimeRepository(PayrollOvertimeContext context) : base(context)
        {
        }
        public Overtime CreateRequest(Overtime overtime)
        {
            try
            {
                var remainingOvertime = RemainingOvertimeByEmployeeGuid(overtime.Employee_id);
                var existingOvertime = ListOvertimeByIdEmployee(overtime.Employee_id).FirstOrDefault(a => a.StartOvertime.Day == overtime.StartOvertime.Day);
                if (remainingOvertime.RemainingOvertime > 0 && existingOvertime == null)
                {
                    var employee = _context.Employees.Where(e => e.Id == overtime.Employee_id)
                                .Join(_context.EmployeeLevels, es => es.EmployeeLevel_id, el => el.Id, (es, el) => new { Employee = es, EmployeeLevel = el }).FirstOrDefault();
                    var salaryPerHours = employee.EmployeeLevel.Salary * 1 / 173;
                    var totalHours = Convert.ToInt32((overtime.EndOvertime - overtime.StartOvertime).TotalHours);
                    var today = overtime.SubmitDate.DayOfWeek;
                    if (today == DayOfWeek.Saturday || today == DayOfWeek.Sunday)
                    {
                        if (totalHours > 11)
                        {
                            totalHours = 11;
                        }
                        overtime.Paid = TotalPaidWeekend(totalHours, salaryPerHours);

                    }
                    else
                    {
                        if (totalHours > 4)
                        {
                            totalHours = 4;
                        }
                        for (int i = 0; i < totalHours; i++)
                        {
                            if (i < 1)
                            {
                                overtime.Paid += Convert.ToInt32(1.5 * salaryPerHours);
                            }
                            else
                            {
                                overtime.Paid += 2 * salaryPerHours;
                            }
                        }
                    }

                    _context.Set<Overtime>().Add(overtime);
                    _context.SaveChanges();
                    return overtime;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        public int TotalPaidWeekend(int totalHours, int salaryPerHours)
        {
            try
            {
                int paid = 0;
                for (int i = 0; i < totalHours; i++)
                {
                    if (i < 8)
                    {
                        paid += 2 * salaryPerHours;
                    }
                    else if (i == 8)
                    {
                        paid += 3 * salaryPerHours;
                    }
                    else
                    {
                        paid += 4 * salaryPerHours;
                    }
                }
                return paid;

            }
            catch
            {
                return 0;
            }
        }
        public int ApprovalOvertime(Overtime overtimeRequest, Guid id)
        {
            try
            {
                var employee = _context.Employees.FirstOrDefault(guid => guid.Id == overtimeRequest.Employee_id);
                var _reportTo = employee.ReportTo;

                if (_reportTo != null && _reportTo == id)
                {
                    _context.Entry(overtimeRequest).State = EntityState.Modified;
                    var result = _context.SaveChanges();
                    return 1;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }
        public List<OvertimeApprovalVM> ListOvertimeByIdManager(Guid managerId)
        {
            try
            {
                var query = _context.Employees
                .Where(e => e.ReportTo == managerId)
                .Join(_context.Overtimes,
                    emp => emp.Id,
                    ov => ov.Employee_id,
                    (emp, ov) => new OvertimeApprovalVM
                    {
                        Id = ov.Id,
                        Fullname = emp.FirstName + " " + emp.LastName,
                        StartOvertime = ov.StartOvertime,
                        EndOvertime = ov.EndOvertime,
                        SubmitDate = ov.SubmitDate,
                        Deskripsi = ov.Deskripsi,
                        Paid = ov.Paid,
                        Status = ov.Status,
                        Employee_id = ov.Employee_id
                    }).Where(sta => sta.Status == 0)
                .ToList();

                return query;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<OvertimeVM> ListOvertimeByIdEmployee(Guid employeeId)
        {
            try
            {
                var overtimes = _context.Overtimes.Where(e => e.Employee_id == employeeId).Select(e => new OvertimeVM
                {
                    Id = e.Id,
                    StartOvertime = e.StartOvertime,
                    EndOvertime = e.EndOvertime,
                    SubmitDate = e.SubmitDate,
                    Deskripsi = e.Deskripsi,
                    Paid = e.Paid,
                    Status = e.Status,
                    Employee_id = e.Employee_id
                }).ToList();

                return overtimes;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<OvertimeRemainingVM> ListRemainingOvertime()
        {
            var today = DateTime.Today;
            var targetDate = new DateTime(today.Year, today.Month, 25);
            var endDate = targetDate.AddDays(-30);
            var overtimeRemaining = _context.Overtimes.Where(a => a.Status == Status.Approved && a.SubmitDate >= endDate && a.SubmitDate <= targetDate)
                .Join(_context.Employees,
                    ov => ov.Employee_id,
                    emp => emp.Id,
                    (ov, emp) => new
                    {
                        guid = emp.Id,
                        fullname = emp.FirstName + " " + emp.LastName,
                        total = (ov.EndOvertime - ov.StartOvertime).TotalHours
                    }).ToList().GroupBy(a => a.guid).Select(b => new OvertimeRemainingVM
                    {
                        Employee_id = b.Key,
                        Fullname = b.First().fullname,
                        RemainingOvertime = Convert.ToInt32(40 - b.Sum(c => c.total))
                    }).ToList();
            return overtimeRemaining;
        }

        public IEnumerable<OvertimeRemainingVM> ListRemainingOvertimeByGuid(Guid id)
        {
            var today = DateTime.Today;
            var targetDate = new DateTime(today.Year, today.Month, 25);
            var endDate = targetDate.AddDays(-30);

            var overtimeRemaining = _context.Overtimes.Where(a => a.Status == Status.Approved && a.SubmitDate >= endDate && a.SubmitDate <= targetDate)
                .Join(_context.Employees.Where(a => a.ReportTo == id),
                    ov => ov.Employee_id,
                    emp => emp.Id,
                    (ov, emp) => new
                    {
                        guid = emp.Id,
                        fullname = emp.FirstName + " " + emp.LastName,
                        total = (ov.EndOvertime - ov.StartOvertime).TotalHours
                    }).ToList().GroupBy(a => a.guid).Select(b => new OvertimeRemainingVM
                    {
                        Employee_id = b.Key,
                        Fullname = b.First().fullname,
                        RemainingOvertime = Convert.ToInt32(40 - b.Sum(c => c.total))
                    }).ToList();

            return overtimeRemaining;
        }
        public OvertimeRemainingVM RemainingOvertimeByEmployeeGuid(Guid id)
        {
            var remaining = ListRemainingOvertime();
            var employeeRemaining = remaining.FirstOrDefault(a => a.Employee_id == id);
            if (employeeRemaining == null)
            {
                var employee = _context.Employees.FirstOrDefault(a => a.Id == id);
                var employeeRemain = new OvertimeRemainingVM();
                employeeRemain.Employee_id = employee.Id;
                employeeRemain.Fullname = employee.FirstName + " " + employee.LastName;
                employeeRemain.RemainingOvertime = 40;

                return employeeRemain;
            }
            return employeeRemaining;
        }

        public ChartVM DataChartByGuid(Guid id)
        {
            try
            {
                var chartManager = new ChartVM();
                var totalMale = _context.Employees.Count(a => a.ReportTo == id && a.Gender == "L");
                var totalFemale = _context.Employees.Count(b => b.ReportTo == id && b.Gender == "P");
                var Approved = _context.Employees
                   .Where(a => a.ReportTo == id)
                    .Join(
                        _context.Overtimes,
                        emp => emp.Id,
                        ov => ov.Employee_id,
                        (emp, ov) => ov
                    )
                    .Count(ov => ov.Status == Status.Approved);

                var rejected = _context.Employees
                   .Where(a => a.ReportTo == id)
                    .Join(
                        _context.Overtimes,
                        emp => emp.Id,
                        ov => ov.Employee_id,
                        (emp, ov) => ov
                    )
                    .Count(ov => ov.Status == Status.Rejected);

                chartManager.Approved = Approved;
                chartManager.Rejected = rejected;
                chartManager.TotalMale = totalMale;
                chartManager.TotalFemale = totalFemale;

                return chartManager;
            }
            catch
            {
                return null;
            }

        }
    }
}