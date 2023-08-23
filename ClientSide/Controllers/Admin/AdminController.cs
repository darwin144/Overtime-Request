using ClientSide.Models;
using ClientSide.Repositories.Interface;
using ClientSide.ViewModel.Account;
using ClientSide.ViewModel.Employee;
using ClientSide.ViewModel.Payroll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace ClientSide.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IEmployeeRepository repository;
        private readonly IEmployeeLevelRepository employeeLevelRepository;
        private readonly IAccountRoleRepository accountRoleRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IPayrollRepository payrollRepository;


        public AdminController(IEmployeeRepository repository,
            IEmployeeLevelRepository employeeLevelRepository,
            IAccountRoleRepository accountRoleRepository,
            IDepartmentRepository departmentRepository,
            IPayrollRepository payrollRepository)
        {
            this.repository = repository;
            this.employeeLevelRepository = employeeLevelRepository;
            this.accountRoleRepository = accountRoleRepository;
            this.departmentRepository = departmentRepository;
            this.payrollRepository = payrollRepository;


        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEmployee()
        {
            var result = await repository.GetAllEmployee();
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

        public async Task<IActionResult> ListEmployee()
        {
            var result = await repository.Get();
            var employees = new List<Employee>();
            if (result.Data != null)
            {
                employees = result.Data?.Select(e => new Employee
                {
                    Id = e.Id,
                    NIK = e.NIK,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    BirthDate = e.BirthDate,
                    Gender = e.Gender,
                    HiringDate = e.HiringDate,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    ReportTo = e.ReportTo,
                    EmployeeLevel_id = e.EmployeeLevel_id,
                    Department_id = e.Department_id,
                }).ToList();
            }
            return View(employees);
        }
        [HttpGet]
        public async Task<IActionResult> CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            var result = await repository.Post(employee);
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

        public async Task<IActionResult> Deletes(Guid Id)
        {
            var result = await repository.Get(Id);
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
            var result = await repository.Deletes(Id);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(GetAllEmployee));
            }
            return RedirectToAction(nameof(GetAllEmployee));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            var result = await repository.Put(employee);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(GetAllEmployee));
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
                /*}*/

            }
            return RedirectToAction(nameof(GetAllEmployee));
        }

        [HttpGet]
        /* [Authorize(Roles = "Admin")]*/
        public async Task<IActionResult> Edit(Guid Id)
        {
            var result = await repository.Get(Id);
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

        // Employee Level 
        public async Task<IActionResult> ListEmployeeLevel()
        {
            var result = await employeeLevelRepository.Get();
            var employeeLevels = new List<EmployeeLevel>();

            if (result.Data != null)
            {
                employeeLevels = result.Data?.Select(e => new EmployeeLevel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Level = e.Level,
                    Salary = e.Salary,
                    Allowence = e.Allowence,

                }).ToList();
            }
            return View(employeeLevels);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEmployeeLevel()
        {
            return View();
        }

        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> CreateEmployeeLevel(EmployeeLevel employeeLevel)
        {
            var result = await employeeLevelRepository.Post(employeeLevel);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(ListEmployeeLevel));
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return RedirectToAction(nameof(ListEmployeeLevel));
        }

        public async Task<IActionResult> DeleteEmployeeLevel(Guid Id)
        {
            var result = await employeeLevelRepository.Get(Id);
            var employeeLevel = new EmployeeLevel();
            if (result.Data?.Id is null)
            {
                return View(employeeLevel);
            }
            else
            {
                employeeLevel.Id = result.Data.Id;
                employeeLevel.Title = result.Data.Title;
                employeeLevel.Level = result.Data.Level;
                employeeLevel.Salary = result.Data.Salary;
                employeeLevel.Allowence = result.Data.Allowence;
            }
            return View(employeeLevel);
        }

        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> RemoveEmployeeLevel(Guid Id)
        {
            var result = await employeeLevelRepository.Deletes(Id);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(ListEmployeeLevel));
            }
            return RedirectToAction(nameof(ListEmployeeLevel));
        }

        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> EditEmployeeLevel(EmployeeLevel employeeLevel)
        {
            var result = await employeeLevelRepository.Put(employeeLevel);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(ListEmployeeLevel));
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return RedirectToAction(nameof(ListEmployeeLevel));
        }

        [HttpGet]
        /*[Authorize(Roles = "Admin")]*/
        public async Task<IActionResult> EditEmployeeLevel(Guid Id)
        {
            var result = await employeeLevelRepository.Get(Id);
            var employeeLevel = new EmployeeLevel();
            if (result.Data?.Id is null)
            {
                return View(employeeLevel);
            }
            else
            {
                employeeLevel.Id = result.Data.Id;
                employeeLevel.Title = result.Data.Title;
                employeeLevel.Level = result.Data.Level;
                employeeLevel.Salary = result.Data.Salary;
                employeeLevel.Allowence = result.Data.Allowence;
            }

            return View(employeeLevel);
        }

        //AccountRole
        public async Task<IActionResult> ListAccountRole()
        {
            var result = await accountRoleRepository.Get();
            var accountRoles = new List<AccountRole>();

            if (result.Data != null)
            {
                accountRoles = result.Data?.Select(e => new AccountRole
                {
                    Id = e.Id,
                    Account_id = e.Account_id,
                    Role_id = e.Role_id,

                }).ToList();
            }
            return View(accountRoles);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAccountRole()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAccountRole(AccountRole accountRole)
        {
            /*if (ModelState.IsValid)
            {*/
            var result = await accountRoleRepository.Post(accountRole);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(GetAllMasterAccountRole));
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            /* }*/
            return RedirectToAction(nameof(GetAllMasterAccountRole));
        }


        [HttpGet]
        public async Task<IActionResult> GetAllMasterAccountRole()
        {
            var result = await accountRoleRepository.GetAllMasterAccountRole();
            var getAllAccountRole = new List<ListAccountRoleVM>();

            if (result.Data != null)
            {
                getAllAccountRole = result.Data.Select(e => new ListAccountRoleVM
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Name = e.Name,
                }).ToList();
            }
            return View(getAllAccountRole);
        }

        [HttpGet]
        public async Task<IActionResult> EditAccountRole(Guid Id)
        {
            var result = await accountRoleRepository.Get(Id);
            var accountRole = new AccountRole();
            if (result.Data?.Id is null)
            {
                return View(accountRole);
            }
            else
            {
                accountRole.Id = result.Data.Id;
                accountRole.Account_id = result.Data.Account_id;
                accountRole.Role_id = result.Data.Role_id;
            }

            return View(accountRole);
        }


        [HttpPost]
        public async Task<IActionResult> EditAccountRole(AccountRole accountRole)
        {
            if (ModelState.IsValid)
            {
                var result = await accountRoleRepository.Put(accountRole);
                if (result.Code == 200)
                {
                    return RedirectToAction(nameof(GetAllMasterAccountRole));
                }
                else if (result.Code == 409)
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View();
                }
            }
            return RedirectToAction(nameof(GetAllMasterAccountRole));
        }

        public async Task<IActionResult> DeletesAccountRole(Guid Id)
        {
            var result = await accountRoleRepository.Get(Id);
            var accountRole = new AccountRole();
            if (result.Data?.Id is null)
            {
                return View(accountRole);
            }
            else
            {
                accountRole.Id = result.Data.Id;
                accountRole.Account_id = result.Data.Account_id;
                accountRole.Role_id = result.Data.Role_id;

            }
            return View(accountRole);
        }

        [HttpPost]
        /* [ValidateAntiForgeryToken]*/
        public async Task<IActionResult> RemoveAccountRole(Guid Id)
        {
            var result = await accountRoleRepository.Deletes(Id);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(GetAllMasterAccountRole));
            }
            return RedirectToAction(nameof(GetAllMasterAccountRole));
        }

        //Department
        public async Task<IActionResult> ListDepartment()
        {
            var result = await departmentRepository.Get();
            var departments = new List<Department>();

            if (result.Data != null)
            {
                departments = result.Data?.Select(e => new Department
                {
                    Id = e.Id,
                    Name = e.Name,

                }).ToList();
            }
            return View(departments);
        }

        [HttpGet]
        public async Task<IActionResult> CreateDepartment()
        {
            return View();
        }

        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> CreateDepartment(Department department)
        {
            var result = await departmentRepository.Post(department);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(ListDepartment));
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return RedirectToAction(nameof(ListDepartment));
        }

        public async Task<IActionResult> DeleteDepartment(Guid Id)
        {
            var result = await departmentRepository.Get(Id);
            var department = new Department();
            if (result.Data?.Id is null)
            {
                return View(department);
            }
            else
            {
                department.Id = result.Data.Id;
                department.Name = result.Data.Name;
            }
            return View(department);
        }
        [HttpPost]
        /* [ValidateAntiForgeryToken]*/
        public async Task<IActionResult> RemoveDepartment(Guid Id)
        {
            var result = await departmentRepository.Deletes(Id);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(ListDepartment));
            }
            return RedirectToAction(nameof(ListDepartment));
        }

        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> EditDepartment(Department department)
        {
            var result = await departmentRepository.Put(department);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(ListDepartment));
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return RedirectToAction(nameof(ListDepartment));
        }
        [HttpGet]
        /* [Authorize(Roles = "Admin")]*/
        public async Task<IActionResult> EditDepartment(Guid Id)
        {
            var result = await departmentRepository.Get(Id);
            var department = new Department();
            if (result.Data?.Id is null)
            {
                return View(department);
            }
            else
            {
                department.Id = result.Data.Id;
                department.Name = result.Data.Name;
            }

            return View(department);
        }



        //Payroll
        public async Task<IActionResult> Payroll()
        {
            /*return View();*/
            var result = await payrollRepository.Get();
            var payrolls = new List<Payroll>();

            if (result.Data != null)
            {
                payrolls = result.Data?.Select(p => new Payroll
                {
                    Id = p.Id,
                    PayDate = p.PayDate,
                    PayrollCuts = p.PayrollCuts,
                    TotalSalary = p.TotalSalary,
                    Employee_id = p.Employee_id
                }).ToList();
            }
            return View(payrolls);
        }


        public async Task<IActionResult> GetAllPayroll()
        {
            var result = await payrollRepository.GetAllPayroll();
            var getAllPayroll = new List<PayrollPrintVM>();

            if (result.Data != null)
            {
                getAllPayroll = result.Data.Select(e => new PayrollPrintVM
                {
                    Id = e.Id,
                    PayDate = e.PayDate,
                    Fullname = e.Fullname,
                    Department = e.Department,
                    Title = e.Title,
                    Allowence = e.Allowence,
                    Overtime = e.Overtime,
                    PayrollCuts = e.PayrollCuts,
                    TotalSalary = e.TotalSalary,
                }).ToList();
            }
            return View(getAllPayroll);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePayroll()
        {
            return View();
        }

        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> CreatePayroll(Payroll payroll)
        {
            var result = await payrollRepository.CreatePayroll(payroll);
            if (result.Code == 200)
            {
                TempData["successMessage"] = "Data Berhasil Disubmit";
                return RedirectToAction(nameof(GetAllPayroll));
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return RedirectToAction(nameof(GetAllPayroll));
        }

        public async Task<IActionResult> DeletePayroll(Guid Id)
        {
            var result = await payrollRepository.Get(Id);
            var payroll = new Payroll();
            if (result.Data?.Id is null)
            {
                return View(payroll);
            }
            else
            {
                payroll.Id = result.Data.Id;
                payroll.PayDate = result.Data.PayDate;
                payroll.PayrollCuts = result.Data.PayrollCuts;
                payroll.TotalSalary = result.Data.TotalSalary;
                payroll.Employee_id = result.Data.Employee_id;
            }
            return View(payroll);
        }
        [HttpPost]
        /* [ValidateAntiForgeryToken]*/
        public async Task<IActionResult> RemovePayroll(Guid Id)
        {
            var result = await payrollRepository.Deletes(Id);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(GetAllPayroll));
            }
            return RedirectToAction(nameof(GetAllPayroll));
        }

        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> EditPayroll(Payroll payroll)
        {
            var result = await payrollRepository.Put(payroll);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(GetAllPayroll));
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return RedirectToAction(nameof(GetAllPayroll));
        }

        [HttpGet]
        /* [Authorize(Roles = "Admin")]*/
        public async Task<IActionResult> EditPayroll(Guid Id)
        {
            var result = await payrollRepository.Get(Id);
            var payroll = new Payroll();
            if (result.Data?.Id is null)
            {
                return View(payroll);
            }
            else
            {
                payroll.Id = result.Data.Id;
                payroll.PayDate = result.Data.PayDate;
                payroll.PayrollCuts = result.Data.PayrollCuts;
                payroll.TotalSalary = result.Data.TotalSalary;
                payroll.Employee_id = result.Data.Employee_id;
            }

            return View(payroll);
        }
    }
}

