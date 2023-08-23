using ClientSide.Models;
using ClientSide.ViewModel.Payroll;
using ClientSide.ViewModel.Response;

namespace ClientSide.Repositories.Interface
{
    public interface IPayrollRepository : IGeneralRepository<Payroll, Guid>
    {
        public Task<ResponseListVM<PayrollPrintVM>> GetAllPayroll();
        Task<ResponseMessageVM> CreatePayroll(Payroll payroll);
    }
}
