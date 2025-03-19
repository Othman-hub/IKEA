using IKEA.BLL.Services.DepartmentServices;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private IDepartmentServices departmentServices;
        public DepartmentController(IDepartmentServices _departmentServices)
        => departmentServices = _departmentServices;
        public IActionResult Index()
        {
            return View();
        }
    }
}
