using IKEA.BLL.Dto_s.Departments;
using IKEA.BLL.Services.DepartmentServices;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentServices departmentServices;
        private readonly ILogger<DepartmentController> logger;
        private readonly IWebHostEnvironment environment;

        public DepartmentController(IDepartmentServices _departmentServices,ILogger<DepartmentController> _logger,IWebHostEnvironment environment)
        {
            departmentServices = _departmentServices;
            logger = _logger;
            this.environment = environment;
        }
        #region Index
        [HttpGet]
        public IActionResult Index() => View(departmentServices.GetAllDepartments());

        #endregion
        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null) return BadRequest();
            var department = departmentServices.GetDepartmentById(id.Value);
            if (department is null) return NotFound();
            return View(department);       
        } 
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(CreatedDepartmenDto departmenDto)
        {
            if (!ModelState.IsValid)
                return View(departmenDto);
            try
            {
                var Result = departmentServices.CreateDepartment(departmenDto);
                if (Result > 0)
                    return RedirectToAction(nameof(Index));
                else
                {
                    ModelState.AddModelError(string.Empty, "Department Is not Created");
                    return View(departmenDto);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                if (environment.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(departmenDto);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An Error Effect at the Creation Operator");
                    return View(departmenDto);
                }
            }

        } 
        #endregion

    }
}
