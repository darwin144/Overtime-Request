using ClientSide.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClientSide.Controllers
{
    [Authorize(Roles = "Manager")]

    /*    [AllowAnonymous]
    */
    public class ManagerController : Controller
    {
        private readonly IManagerRepository _repository;

        public ManagerController(IManagerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var id = HttpContext.Session.GetString("id");

            if (token == null || id == null)
            {
                return StatusCode(401);
            }

            ViewBag.Token = token;
            ViewBag.Id = id;

            var claims = ExtractClaims(token);
            var guidEmployee = claims.Where(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid").Select(s => s.Value).Single();
            var idClaim = claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid")?.Value;
            
            if (Guid.TryParse(idClaim, out Guid userId))
            {
                var managerResponse = await _repository.GetEmployeeById(userId);

                if (managerResponse != null && managerResponse.Data != null)
                {
                    var manager = managerResponse.Data;
                    ViewBag.FullName = manager.FirstName + " " + manager.LastName;
                    ViewBag.Email = manager.Email;
                }
            }

            return View();
        }

        public IActionResult List()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var claim = _repository.ExtractClaims(token);
            var guidEmployee = claim.Where(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid").Select(s => s.Value).Single();

            ViewData["guidEmployee"] = guidEmployee;
            ViewData["token"] = token;
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var id = HttpContext.Session.GetString("id");

            if (token != null && id != null)
            {
                ViewBag.Token = token;
                ViewBag.Id = id;

                var claims = ExtractClaims(token);
                var idClaim = claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid")?.Value;
                var roleClaim = claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

                if (Guid.TryParse(idClaim, out Guid userId) && roleClaim == "Manager")
                {
                    var managerResponse = await _repository.GetEmployeeById(userId);

                    if (managerResponse != null && managerResponse.Data != null)
                    {
                        var manager = managerResponse.Data;
                        ViewBag.FirstName = manager.FirstName;
                        ViewBag.LastName = manager.LastName;
                        ViewBag.Gender = manager.Gender;
                        ViewBag.PhoneNumber = manager.PhoneNumber;
                        ViewBag.Nik = manager.Nik;
                        ViewBag.Email = manager.Email;
                        ViewBag.HiringDate = manager.HiringDate.ToString("yyyy-MM-dd");
                        ViewBag.BirthDate = manager.BirthDate.ToString("yyyy-MM-dd");
                    }
                }
            }

            return View();
        }
        public IActionResult Payslip()
        {
            return View();
        }

        public IEnumerable<Claim> ExtractClaims(string jwtToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);
            IEnumerable<Claim> claims = securityToken.Claims;
            return claims;
        }
    }
}
