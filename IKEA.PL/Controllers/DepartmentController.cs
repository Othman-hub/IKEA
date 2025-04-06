using IKEA.BLL.Dto_s.Departments;
using IKEA.BLL.Services.DepartmentServices;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class DepartmentController : Controller
    {
        #region Services - DI
        private readonly IDepartmentServices departmentServices;
        private readonly ILogger<DepartmentController> logger;
        private readonly IWebHostEnvironment environment;

        public DepartmentController(IDepartmentServices _departmentServices, ILogger<DepartmentController> _logger, IWebHostEnvironment environment)
        {
            departmentServices = _departmentServices;
            logger = _logger;
            this.environment = environment;
        } 
        #endregion

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

        #region Update
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest();
            var department = departmentServices.GetDepartmentById(id.Value);
            if (department is null) return NotFound();
            var MappedDepartment = new UpdatedDepartmentDto()
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code,
                Description = department.Description,
                CreationDate = department.CreationDate,
            };
            return View(MappedDepartment);
        }
        [HttpPost]
        public IActionResult Edit(UpdatedDepartmentDto departmentDto)
        {
            if (!ModelState.IsValid) return View(departmentDto);
            var Message = string.Empty;
            try
            {
                var result = departmentServices.UpdateDepartment(departmentDto);
                if (result > 0) return RedirectToAction(nameof(Index));
                else Message = "Department is Not Upbdated";
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                Message = environment.IsDevelopment() ? ex.Message : "An Error Has been occurd during Ubdate the Department!";
            }
            ModelState.AddModelError(string.Empty, Message);
            return View(departmentDto);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            var department = departmentServices.GetDepartmentById(id.Value);
            if (department is null) return NotFound();

            return View(department);
        }
        [HttpPost]
        public IActionResult Delete(int Did)
        {
            var Message = string.Empty;
            try
            {
                var IsDeleted = departmentServices.DeleteDepartment(Did);
                if (IsDeleted) return RedirectToAction(nameof(Index));
                Message = "Department is not Deleted";
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                Message = environment.IsDevelopment() ? ex.Message : "An Error has been occured during delete the Department!";
            }
            ModelState.AddModelError(string.Empty, Message);
            return RedirectToAction(nameof(Delete), new { id = Did });
        }
        #endregion
    }
}
