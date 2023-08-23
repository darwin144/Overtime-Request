using APIs.Contract;
using APIs.Model;
using APIs.ViewModel.Accounts;
using APIs.ViewModel.Employees;
using APIs.ViewModel.Others;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace APIs.Controllers
{
    public class AccountController : BaseController<Account, AccountVM>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper<Account, AccountVM> _mapper;
        public AccountController(IMapper<Account, AccountVM> mapper, IAccountRepository accountRepository, IEmployeeRepository employeeRepository, ITokenService tokenService, IEmailService emailService, IConfiguration configuration) 
            : base(accountRepository, mapper)
        {
            _accountRepository = accountRepository;
            _employeeRepository = employeeRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _configuration = configuration;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {

            var result = await _accountRepository.Register(registerVM);
            
            switch (result)
            {
                case 0:
                    return BadRequest(new ResponseVM<RegisterVM>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Registration Failed",
                    });
                case 1:
                    return BadRequest(new ResponseVM<RegisterVM>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Email Already Exist",
                    });
                case 2:
                    return BadRequest(new ResponseVM<RegisterVM>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Phone Number Already Exist",
                    });
                case 3:
                    return Ok(new ResponseVM<RegisterVM>
                    {
                        Code = StatusCodes.Status200OK,
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Proses Register Berhasil",
                    });
            }

            return Ok();

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginVM loginVM)
        {
            try
            {

                var result = await _accountRepository.Login(loginVM);
                if (!result)
                {
                    return NotFound(new ResponseVM<LoginVM>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Data Akun Tidak Ditemukan",
                    });
                }

                var userdata = await _accountRepository.GetUserData(loginVM.Email);
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.PrimarySid, userdata.GuidEmployee),
                    new Claim(ClaimTypes.Email, userdata.Email),
                    new Claim(ClaimTypes.Name, userdata.FullName)
                };

                var getRoles = await _accountRepository.GetRoles(loginVM.Email);
                foreach (var item in getRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signIn
                    );

                var generateToken = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new ResponseVM<string>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Berhasil Login",
                    Data = generateToken

                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseVM<LoginVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = ex.Message,
                });

            }
        }
        [AllowAnonymous]
        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordVM changePasswordVM)
        {
            // Cek apakah email dan OTP valid
            var account = _employeeRepository.FindEmployeeByEmail(changePasswordVM.Email);
            var changePass = _accountRepository.ChangePasswordAccount(account.Id, changePasswordVM);
            switch (changePass)
            {
                case 0:
                    return BadRequest(new ResponseVM<ChangePasswordVM>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Change Password Failed",
                    });
                case 1:
                    return Ok(new ResponseVM<ChangePasswordVM>
                    {
                        Code = StatusCodes.Status200OK,
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Password Change Succesful",
                    });
                case 2:
                    return BadRequest(new ResponseVM<ChangePasswordVM>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Invalid OTP",
                    });
                case 3:
                    return BadRequest(new ResponseVM<ChangePasswordVM>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "OTP Has Already Been Used",
                    });
                /*case 4:
                    return BadRequest("OTP expired");*/
                case 5:
                    return BadRequest(new ResponseVM<ChangePasswordVM>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Cek..",
                    });

            }
            return null;

        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword/{email}")]
        public IActionResult ForgotPassword(String email)
        {
            var employee = _employeeRepository.FindEmployeeByEmail(email);
            if (employee.Id == null)
            {
                return NotFound(new ResponseVM<AccountVM>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Akun Tidak Ditemukan",
                });
            }

            var isUpdated = _accountRepository.UpdateOTP(employee.Id);

            switch (isUpdated)
            {
                case 0:
                    return BadRequest(new ResponseVM<AccountVM>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Failed Update OTP"
                    });
                default:

                    _emailService.SetEmail(email)
                            .SetSubject("Forget Password")
                            .SetHtmlMessage($"Your OTP is {isUpdated}")
                            .SendEmailAsync();

                    return Ok(new ResponseVM<ResetPasswordVM>
                    {
                        Code = StatusCodes.Status200OK,
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "OTP Succesfully Sent to Email",
                    });
             }
        }
    }
}
