using AutoMapper;
using IKEA.BLL.Dto_s.Departments;
using IKEA.BLL.Services.DepartmentServices;
using IKEA.DAL.Models.Departments;
using IKEA.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class DepartmentController : Controller
    {
        #region Services - DI
        private readonly IDepartmentServices departmentServices;
        private readonly IMapper mapper;
        private readonly ILogger<DepartmentController> logger;
        private readonly IWebHostEnvironment environment;

        public DepartmentController(IDepartmentServices _departmentServices,IMapper mapper, ILogger<DepartmentController> _logger, IWebHostEnvironment environment)
        {
            departmentServices = _departmentServices;
            this.mapper = mapper;
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
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentVM departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);
            string Message = string.Empty;
            try
            {
                
                var Result = departmentServices.CreateDepartment(mapper.Map<DepartmentVM,CreatedDepartmenDto>(departmentVM));
                if (Result > 0)
                    return RedirectToAction(nameof(Index));
                else
                    Message = "Department Is not Created";
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                    Message = environment.IsDevelopment() ? ex.Message: "An Error Effect at the Creation Operator";
            }
            ModelState.AddModelError(string.Empty, Message);
            return View(departmentVM);

        }
        #endregion

        #region Update
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest();
            var department = departmentServices.GetDepartmentById(id.Value);
            if (department is null) return NotFound();
            //var MappedDepartment = new DepartmentVM()
            //{
            //    Id = department.Id,
            //    Name = department.Name,
            //    Code = department.Code,
            //    Description = department.Description,
            //    CreationDate = department.CreationDate,
            //};
            return View(mapper.Map<DepartmentDetailsDto,DepartmentVM>(department));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DepartmentVM departmentVM)
        {
            if (!ModelState.IsValid) return View(departmentVM);
            var Message = string.Empty;
            try
            {
                var result = departmentServices.UpdateDepartment(mapper.Map<DepartmentVM,UpdatedDepartmentDto>(departmentVM));
                if (result > 0) return RedirectToAction(nameof(Index));
                else Message = "Department is Not Upbdated";
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                Message = environment.IsDevelopment() ? ex.Message : "An Error Has been occurd during Ubdate the Department!";
            }
            ModelState.AddModelError(string.Empty, Message);
            return View(departmentVM);
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
        [ValidateAntiForgeryToken]
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
