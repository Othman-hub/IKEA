using IKEA.BLL.Dto_s.Departments;
using IKEA.BLL.Dto_s.Employees;
using IKEA.BLL.Services.DepartmentServices;
using IKEA.BLL.Services.EmployeeServices;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class EmployeeController : Controller
    {
        #region Services - DI
        private readonly IEmployeeServices employeeServices;
        private readonly ILogger<EmployeeController> logger;
        private readonly IWebHostEnvironment environment;

        public EmployeeController(IEmployeeServices employeeServices, ILogger<EmployeeController> logger, IWebHostEnvironment environment)
        {
            this.employeeServices = employeeServices;
            this.logger = logger;
            this.environment = environment;
        }
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index() =>
            View(employeeServices.GetAllEmployees());
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(CreatedEmployeeDto EmployeeDto)
        {
            if (!ModelState.IsValid)
                return View(EmployeeDto);
            string Message = string.Empty;
            try
            {
                var Result = employeeServices.CreateEmployee(EmployeeDto);
                if (Result > 0)
                    return RedirectToAction(nameof(Index));
                else
                    Message = "Employee Is not Created";
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                Message = environment.IsDevelopment() ? ex.Message : "An Error Effect at the Creation Operator";
            }
            ModelState.AddModelError(string.Empty, Message);
            return View(EmployeeDto);
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null) return BadRequest();
            var employee = employeeServices.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();
            return View(employee);
        }
        #endregion

        #region Update
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest();
            var employee = employeeServices.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();
            var MappedEmployee = new UpdatedEmployeeDto()
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Address = employee.Address,
                HiringDate = employee.HiringDate,
                Salary = employee.Salary,
                Gender = employee.Gender,
                EmployeeType = employee.EmployeeType,
                IsActive = employee.IsActive,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber 
            };
            return View(MappedEmployee);
        }
        [HttpPost]
        public IActionResult Edit(UpdatedEmployeeDto employeeDto)
        {
            if (!ModelState.IsValid) return View(employeeDto);
            var Message = string.Empty;
            try
            {
                var result = employeeServices.UpdateEmployee(employeeDto);
                if (result > 0) return RedirectToAction(nameof(Index));
                else Message = "Employee is Not Upbdated";

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                Message = environment.IsDevelopment() ? ex.Message : "An Error Has been occurd during Ubdate the Employee!";
            }
            ModelState.AddModelError(string.Empty, Message);
            return View(employeeDto);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            var employee = employeeServices.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();

            return View(employee);
        }
        [HttpPost]
        public IActionResult Delete(int EmpId)
        {
            var Message = string.Empty;
            try
            {
                var IsDeleted = employeeServices.DeleteEmployee(EmpId);
                if (IsDeleted) return RedirectToAction(nameof(Index));
                Message = "Employee is not Deleted";
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                Message = environment.IsDevelopment() ? ex.Message : "An Error has been occured during delete the Employee!";
            }
            ModelState.AddModelError(string.Empty, Message);
            return RedirectToAction(nameof(Delete), new { id = EmpId });
        }
        #endregion
    }
}
