using IKEA.BLL.Dto_s.Departments;
using IKEA.BLL.Dto_s.Employees;
using IKEA.BLL.Services.DepartmentServices;
using IKEA.BLL.Services.EmployeeServices;
using IKEA.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IKEA.PL.Controllers
{
    public class EmployeeController : Controller
    {
        #region Services - DI
        private readonly IEmployeeServices employeeServices;
        private readonly ILogger<EmployeeController> logger;
        private readonly IWebHostEnvironment environment;

        public EmployeeController(IEmployeeServices employeeServices,IDepartmentServices departmentServices, ILogger<EmployeeController> logger, IWebHostEnvironment environment)
        {
            this.employeeServices = employeeServices;
            this.logger = logger;
            this.environment = environment;
        }
        #endregion

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index(string search) =>
            View(await employeeServices.GetAllEmployees(search));
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeVM employeeVM)
        {
            if (!ModelState.IsValid)
                return View(employeeVM);
            string Message = string.Empty;
            try
            {
                var Result = await employeeServices.CreateEmployee(new CreatedEmployeeDto()
                {
                    Name = employeeVM.Name,
                    Age = employeeVM.Age,
                    Address = employeeVM.Address,
                    Salary = employeeVM.Salary,
                    IsActive = employeeVM.IsActive,
                    Email = employeeVM.Email,
                    PhoneNumber = employeeVM.PhoneNumber,
                    HiringDate = employeeVM.HiringDate,
                    Gender = employeeVM.Gender,
                    EmployeeType = employeeVM.EmployeeType,
                    DepartmentId = employeeVM.DepartmentId,
                    Image = employeeVM.Image,
                });
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
            return View(employeeVM);
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return BadRequest();
            var employee = await employeeServices.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();
            return View(employee);
        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();
            var employee = await employeeServices.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();
            var MappedEmployee = new EmployeeVM()
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
                PhoneNumber = employee.PhoneNumber,
                ImageName = employee.ImageName
            };
            
            return View(MappedEmployee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeVM employeeVM)
        {
            if (!ModelState.IsValid) return View(employeeVM);
            var Message = string.Empty;
            try
            {
                var result = await employeeServices.UpdateEmployee(new UpdatedEmployeeDto()
                {
                    Id = employeeVM.Id,
                    Name = employeeVM.Name,
                    Age = employeeVM.Age,
                    Address = employeeVM.Address,
                    Salary = employeeVM.Salary,
                    IsActive = employeeVM.IsActive,
                    Email = employeeVM.Email,
                    PhoneNumber = employeeVM.PhoneNumber,
                    HiringDate = employeeVM.HiringDate,
                    Gender = employeeVM.Gender,
                    EmployeeType = employeeVM.EmployeeType,
                    DepartmentId = employeeVM.DepartmentId,
                    Image = employeeVM.Image,
                    ImageName = employeeVM.ImageName
                });
                if (result > 0) return RedirectToAction(nameof(Index));
                else Message = "Employee is Not Upbdated";

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                Message = environment.IsDevelopment() ? ex.Message : "An Error Has been occurd during Ubdate the Employee!";
            }
            ModelState.AddModelError(string.Empty, Message);
            return View(employeeVM);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var employee = await employeeServices.GetEmployeeById(id.Value);
            if (employee is null) return NotFound();

            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int EmpId)
        {
            var Message = string.Empty;
            try
            {
                var IsDeleted = await employeeServices.DeleteEmployee(EmpId);
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
