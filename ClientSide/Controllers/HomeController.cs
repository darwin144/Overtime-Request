using ClientSide.Repositories.Data;
using ClientSide.ViewModel.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClientSide.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly HomeRepository _repository;

        public HomeController(HomeRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Dashboard()
        {

            return View();
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            var jwtToken = await _repository.Logins(login);

            if (jwtToken == null || jwtToken.Data == null)
            {
                TempData["ErrorMessage"] = "Email or password is incorrect.";
                return RedirectToAction("Login", "Home");
            }
            var token = jwtToken.Data;
            var claim = ExtractClaims(token);
            var role = claim.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Select(a => a.Value).LastOrDefault();
            var idClaim = claim.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid");
            var id = idClaim != null ? idClaim.Value : null;
            if (token == null)
            {
                return RedirectToAction("forgotpassword", "Home");
            }

            HttpContext.Session.SetString("JWToken", token);
            HttpContext.Session.SetString("id", id);

            if (role.Contains("Employee")) return RedirectToAction("Profile", "Employee");
            if (role.Contains("Manager")) return RedirectToAction("Index", "Manager");
            if (role.Contains("Admin")) return RedirectToAction("GetAllEmployee", "AdminEmployee");
            else return RedirectToAction("Index", "Home");

        }

        public IEnumerable<Claim> ExtractClaims(string jwtToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);
            IEnumerable<Claim> claims = securityToken.Claims;
            return claims;
        }

        [HttpGet("/Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Dashboard", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            var result = await _repository.Registers(registerVM);

            if (result is null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                TempData["Error"] = $"Something Went Wrong! - {result.Message}!";
                return View();
            }
            else if (result.Code == 200)
            {
                TempData["Success"] = $"Data has been Successfully Registered! - {result.Message}!";
                return RedirectToAction("GetAllEmployee", "Admin");
            }
            return RedirectToAction("GetAllEmployee", "AdminEmployee");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpGet("/Unauthorized")]
        public IActionResult Unauthorized()
        {
            return View("401");
        }

        [AllowAnonymous]
        [HttpGet("/notfound")]
        public IActionResult NotFound()
        {
            return View("404");
        }

        [AllowAnonymous]
        [HttpGet("/forbidden")]
        public IActionResult Forbidden()
        {
            return View("403");
        }
    }
}