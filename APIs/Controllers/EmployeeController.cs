using APIs.Contract;
using APIs.Model;
using APIs.ViewModel.Employees;
using APIs.ViewModel.Others;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APIs.Controllers
{
    [ApiController]
    [Route("API-Overtimes/[controller]")]
    public class EmployeeController : BaseController<Employee, EmployeeVM>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper<Employee, EmployeeVM> _mapper;


        public EmployeeController(IEmployeeRepository employeeRepository, IMapper<Employee, EmployeeVM> mapper) : base(employeeRepository, mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }
        [HttpGet("GetAllMasterEmployee")]
        public IActionResult GetAllMasterEmployee()
        {
            var masterEmployees = _employeeRepository.GetAllMasterEmployee();
            if (!masterEmployees.Any())
            {
                return NotFound(new ResponseVM<MasterEmployeeVM>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Employee Tidak Ditemukan",
                });
            }

            return Ok(new ResponseVM<IEnumerable<MasterEmployeeVM>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Master Employee Berhasil Ditampilkan",
                Data = masterEmployees
            });
        }
    }
}
