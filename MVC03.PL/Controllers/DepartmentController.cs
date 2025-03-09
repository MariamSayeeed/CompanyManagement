using Microsoft.AspNetCore.Mvc;
using MVC03.BLL.Interfaces;
using MVC03.BLL.Repositories;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;


namespace MVC03.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _deptRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _deptRepository = departmentRepository;
        }

        [HttpGet]  // GET ://Department//Index
        public IActionResult Index()
        {
            var departments = _deptRepository.GetAll();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // server side validation
            {
                var department = new Department()
                {
                    Name = model.Name,
                    Code = model.Code,
                    CreateAt = model.CreateAt
                };
                var count = _deptRepository.Add(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
        }


        //[HttpGet]
        //public IActionResult Edit(Department _department)
        //{
        //    var deprtment = _deptRepository.Update(_department);
        //    return View(deprtment);
        //}

        [HttpGet]
        public IActionResult Details(int? id )
        {
            if (id is null) return BadRequest("Invalid Id ");
           
            var deprtment = _deptRepository.Get(id.Value);

            if (deprtment == null) return NotFound(new { statusCode =400 , messege = $"Department With Id:{id} is Not Found" });   

            return View(deprtment);
        }

    }
}
