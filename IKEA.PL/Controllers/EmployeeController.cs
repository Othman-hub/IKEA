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
    }
}
