using Microsoft.AspNetCore.Mvc;
using MVC03.BLL.Interfaces;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;

namespace MVC03.PL.Controllers
{
    public class EmployeeController : Controller 
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IDepartmentRepository _departmentRepo;
        public EmployeeController(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepo)
        {
            _employeeRepo = employeeRepository;
            _departmentRepo = departmentRepo;
        }
        public IActionResult Index()
        {
            // Dictionary : 
            // Storage => ViewData , ViewBag , TempData

            // 1. ViewData : transfer extra info from action to view
            // 2. ViewBag : transfer extra info from action to view &&  Safety Type ==> 'Generic'
            // 3. TempData : transfer extra info from one request to another

            var employees = _employeeRepo.GetAll();
            ViewData["Message"] = "Hello from ViewData";
            ViewBag.Message2 = "Hello from ViewBag";
            ViewBag.NewType = new { message = "Anoynomous Type" };
            return View(employees);


        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var departments = _departmentRepo.GetAll();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if(ModelState.IsValid )
            {
                var employee = new Employee()
                {
                    Name = model.Name,
                    Salary = model.Salary,
                    Address = model.Address,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    Age = model.Age,

                    HiringDate = model.HiringDate,
                    Phone = model.Phone,
                    CreateAt = model.CreateAt,
                    Email = model.Email,

                };
                var count = _employeeRepo.Add(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created Success";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Details(int? id , string viewname = "Details")
        {
            if (id is null) return BadRequest();
            var employee = _employeeRepo.Get(id.Value); 

            if (employee == null) return NotFound(new { statusCode = 400, messege = $"Employee With Id:{id} is Not Found" });


            return View(viewname, employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest();
            var employee = _employeeRepo.Get(id.Value);

            if (employee == null) return NotFound(new { statusCode = 400, messege = $"Employee With Id:{id} is Not Found" });
            var employeeDto = new CreateEmployeeDto
            {
                Email = employee.Email,
                Phone = employee.Phone,
                Address = employee.Address,
                Age = employee.Age,
                HiringDate = employee.HiringDate,
                Name = employee.Name,
                IsActive = employee.IsActive,
                IsDeleted = employee.IsDeleted,
                CreateAt = employee.CreateAt,
                Salary = employee.Salary
            };


            return View(employeeDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    Id = id,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    Age = model.Age,
                    HiringDate = model.HiringDate,
                    Name = model.Name,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    CreateAt = model.CreateAt,
                    Salary = model.Salary
                };


                var count = _employeeRepo.Update(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var employee = _employeeRepo.Get(id);

        //        if (employee == null) return NotFound(new { statusCode = 400, messege = $"Employee With Id:{id} is Not Found" });


        //employee.Email = model.Email;
        //            employee.Phone = model.Phone;
        //            employee.Address = model.Address;
        //            employee.Age = model.Age;
        //            employee.HiringDate = model.HiringDate;
        //            employee.Name = model.Name;
        //            employee.IsActive = model.IsActive;
        //            employee.IsDeleted = model.IsDeleted;
        //            employee.CreateAt = model.CreateAt;
        //            employee.Salary = model.Salary;

        //        var count = _employeeRepo.Update(employee);
        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }

        //    }
        //    return View(model);
        //}


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee = _employeeRepo.Get(id);

                if (employee == null) return NotFound(new { statusCode = 400, messege = $"Employee With Id:{id} is Not Found" });

               
                var count = _employeeRepo.Delete(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                
            }
            return View(model);
        }



    }
}
