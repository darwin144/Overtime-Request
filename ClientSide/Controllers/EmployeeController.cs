using ClientSide.Models;
using ClientSide.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClientSide.Controllers
{
    [Authorize(Roles = "Employee")]

    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IOvertimeRepository _overtimeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IOvertimeRepository overtimeRepository)
        {
            _employeeRepository = employeeRepository;
            _overtimeRepository = overtimeRepository;
        }
        public IActionResult Payslip()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var claim = ExtractClaims(token);
            var guidEmployee = claim.Where(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid").Select(s => s.Value).Single();

            ViewData["guidEmployee"] = guidEmployee;
            ViewData["token"] = token;
            return View();
        }
        public IActionResult Request()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var claim = ExtractClaims(token);
            var guidEmployee = claim.Where(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid").Select(s => s.Value).Single();

            ViewData["guidEmployee"] = guidEmployee;
            ViewData["token"] = token;
            return View();
        }
        /*[Authorize(Roles = "Employee")]*/
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


                if (Guid.TryParse(idClaim, out Guid userId))
                {
                    var employeeResponse = await _employeeRepository.GetEmployeeById(userId);

                    if (employeeResponse != null && employeeResponse.Data != null)
                    {
                        var employee = employeeResponse.Data;
                        ViewBag.FirstName = employee.FirstName;
                        ViewBag.LastName = employee.LastName;
                        ViewBag.Gender = employee.Gender;
                        ViewBag.PhoneNumber = employee.PhoneNumber;
                        ViewBag.Nik = employee.Nik;
                        ViewBag.Email = employee.Email;
                        ViewBag.HiringDate = employee.HiringDate.ToString("yyyy-MM-dd");
                        ViewBag.BirthDate = employee.BirthDate.ToString("yyyy-MM-dd");
                    }
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult CreateOvertime()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOvertime(Overtime overtime)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");
                var claim = ExtractClaims(token);
                var role = claim.Where(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Select(s => s.Value).ToList();
                var guidEmployee = claim.Where(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid").Select(s => s.Value).Single();

                overtime.Employee_id = Guid.Parse(guidEmployee);
                overtime.SubmitDate = DateTime.Today;

                var result = await _overtimeRepository.RequestOvertime(overtime);
                if (result.Code == 200)
                {
                    TempData["successMessage"] = "Data Berhasil Disubmit";
                    return RedirectToAction("Request", "Employee");
                }

                return RedirectToAction("Request", "Employee");

            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }

        }
        /*[Authorize(Roles = "Employee")]*/
        [HttpGet]
        public async Task<IActionResult> GetAllOvertimeById()
        {

            var token = HttpContext.Session.GetString("JWToken");
            var claim = ExtractClaims(token);
            var guidEmployee = claim.Where(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid").Select(s => s.Value).Single();
            var fullname = claim.Where(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Select(s => s.Value).Single();

            var overtimes = await _overtimeRepository.GetOvertimeByemployeeGuid(Guid.Parse(guidEmployee));


            if (overtimes != null)
            {
                foreach (var item in overtimes)
                {
                    item.FullName = fullname;
                }
                return View("GetAllOvertimeById", overtimes);
            }


            return RedirectToAction("Error", "Home");
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
