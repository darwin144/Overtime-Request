using APIs.Contract;
using APIs.Model;
using APIs.ViewModel.Departments;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [ApiController]
    [Route("API-Overtimes/[controller]")]
    public class DepartmentController : BaseController<Department, DepartmentVM>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper<Department, DepartmentVM> _mapper;

        public DepartmentController(IDepartmentRepository departmentRepository, IMapper<Department, DepartmentVM> mapper) : base(departmentRepository, mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
    }
}
