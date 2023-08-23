using APIs.Contract;
using APIs.Model;
using APIs.ViewModel.Roles;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [ApiController]
    [Route("API-Overtimes/[controller]")]
    public class RoleController : BaseController<Role, RoleVM>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper<Role, RoleVM> _mapper;

        public RoleController(IRoleRepository roleRepository, IMapper<Role, RoleVM> mapper) : base(roleRepository, mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }
    }
}