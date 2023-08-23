using APIs.Contract;
using APIs.Model;
using APIs.ViewModel.EmployeeLevels;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [ApiController]
    [Route("API-Payroll/[controller]")]
    public class EmployeeLevelController : BaseController<EmployeeLevel, EmployeeLevelsVM>
    {

        private readonly IEmployeeLevelRepository _employeeLevelRepository;
        private readonly IMapper<EmployeeLevel, EmployeeLevelsVM> _mapper;

        public EmployeeLevelController(IEmployeeLevelRepository employeeLevelRepository, IMapper<EmployeeLevel, EmployeeLevelsVM> mapper) : base(employeeLevelRepository, mapper)
        {
            _employeeLevelRepository = employeeLevelRepository;
            _mapper = mapper;
        }
    }
}
