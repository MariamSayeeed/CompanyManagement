using Microsoft.AspNetCore.Mvc;
using MVC03.BLL.Repositories;

namespace MVC03.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly DepartmentRepository _deptRepository;

        public DepartmentController(DepartmentRepository departmentRepository)
        {
             _deptRepository = departmentRepository;
        }

        [HttpGet]  // GET ://Department//Index
        public IActionResult Index()
        {
            var departments = _deptRepository.GetAll();
            return View(departments);
        }
    }
}
