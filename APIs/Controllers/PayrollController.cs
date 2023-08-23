using APIs.Contract;
using APIs.Model;
using APIs.ViewModel.Others;
using APIs.ViewModel.Payrolls;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace APIs.Controllers
{
    [ApiController]
    [Route("API-Overtimes/[controller]")]
    public class PayrollController : BaseController<Payroll, PayrollVM>
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IMapper<Payroll, PayrollVM> _mapper;

        public PayrollController(IPayrollRepository payrollRepository, IMapper<Payroll, PayrollVM> mapper) 
            : base( payrollRepository, mapper)
        {
            _payrollRepository = payrollRepository;
            _mapper = mapper;
        }
        [HttpGet("GetAllDetails")]
        public IActionResult GetListDetails()
        {
            try
            {
                var result = _payrollRepository.GetAllDetailPayrolls();
                if (result is null)
                {
                    return NotFound(new ResponseVM<PayrollCreateVM>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Pengambilan Data Gagal"
                    });
                }
                return Ok(new ResponseVM<IEnumerable<PayrollPrintVM>>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Data Berhasil Diambil",
                    Data = result
                });
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetByEmployeeId/{guid}")]
        public IActionResult GetByEmployeeId(Guid guid)
        {
            try
            {
                var result = _payrollRepository.GetAllDetailPayrollsByEmployeeID(guid);
                if (result is null)
                {
                    return NotFound(new ResponseVM<PayrollCreateVM>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Pengambilan Data Gagal"
                    });
                }
                return Ok(new ResponseVM<IEnumerable<PayrollPrintVM>>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Data Berhasil Diambil",
                    Data = result
                });
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
