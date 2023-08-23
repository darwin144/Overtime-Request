using APIs.Model;
using APIs.ViewModel.Payrolls;

namespace APIs.Contract
{
    public interface IPayrollRepository : IGenericRepository<Payroll>
    {
        Payroll CreatePayroll(PayrollCreateVM payrollCreate);
        IEnumerable<PayrollPrintVM> GetAllDetailPayrolls();
        IEnumerable<PayrollPrintVM> GetAllDetailPayrollsByEmployeeID(Guid id);
    }
}
