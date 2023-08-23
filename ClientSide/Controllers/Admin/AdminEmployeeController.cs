using ClientSide.Models;
using ClientSide.Repositories.Interface;
using ClientSide.ViewModel.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientSide.Controllers.Admin
{
    public class AdminEmployeeController : Controller
    {

        private readonly IEmployeeRepository _context;

        public AdminEmployeeController(IEmployeeRepository context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEmployee()
        {
            var result = await _context.GetAllEmployee();
            var getAllEmployee = new List<ListEmployeeVM>();

            if (result.Data != null)
            {
                getAllEmployee = result.Data.Select(e => new ListEmployeeVM
                {
                    Id = e.Id,
                    NIK = e.NIK,
                    FullName = e.FullName,
                    BirthDate = e.BirthDate,
                    Gender = e.Gender,
                    HiringDate = e.HiringDate,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    ReportTo = e.ReportTo,
                    Title = e.Title,
                    DepartmentName = e.DepartmentName,
                }).ToList();
            }
            return View(getAllEmployee);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            var result = await _context.Post(employee);
            if (result.Code == 200) return RedirectToAction(nameof(GetAllEmployee));            
            if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return RedirectToAction(nameof(GetAllEmployee));
        }

        public async Task<IActionResult> Deletes(Guid Id)
        {
            var result = await _context.Get(Id);
            var employee = new Employee();
            if (result.Data?.Id is null)
            {
                return View(employee);
            }
            else
            {
                employee.Id = result.Data.Id;
                employee.NIK = result.Data.NIK;
                employee.FirstName = result.Data.FirstName;
                employee.LastName = result.Data.LastName;
                employee.BirthDate = result.Data.BirthDate;
                employee.Gender = result.Data.Gender;
                employee.HiringDate = result.Data.HiringDate;
                employee.Email = result.Data.Email;
                employee.PhoneNumber = result.Data.PhoneNumber;
                employee.ReportTo = result.Data.ReportTo;
                employee.EmployeeLevel_id = result.Data.EmployeeLevel_id;
                employee.Department_id = result.Data.Department_id;

            }
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> Remove(Guid Id)
        {
            var result = await _context.Deletes(Id);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(GetAllEmployee));
            }
            return RedirectToAction(nameof(GetAllEmployee));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            var result = await _context.Put(employee);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(GetAllEmployee));
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();

            }
            return RedirectToAction(nameof(GetAllEmployee));
        }

        [HttpGet]
        /* [Authorize(Roles = "Admin")]*/
        public async Task<IActionResult> Edit(Guid Id)
        {
            var result = await _context.Get(Id);
            var employee = new Employee();
            if (result.Data?.Id is null)
            {
                return View(employee);
            }
            else
            {
                employee.Id = result.Data.Id;
                employee.NIK = result.Data.NIK;
                employee.FirstName = result.Data.FirstName;
                employee.LastName = result.Data.LastName;
                employee.BirthDate = result.Data.BirthDate;
                employee.Gender = result.Data.Gender;
                employee.HiringDate = result.Data.HiringDate;
                employee.Email = result.Data.Email;
                employee.PhoneNumber = result.Data.PhoneNumber;
                employee.ReportTo = result.Data.ReportTo;
                employee.EmployeeLevel_id = result.Data.EmployeeLevel_id;
                employee.Department_id = result.Data.Department_id;
            }

            return View(employee);
        }

    }
}
