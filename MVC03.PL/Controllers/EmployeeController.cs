using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC03.BLL.Interfaces;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;
using MVC03.PL.Helpers;

namespace MVC03.PL.Controllers
{
    public class EmployeeController : Controller 
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepo;
        //private readonly IDepartmentRepository _departmentRepo;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork,
            //IEmployeeRepository employeeRepository,

            //IDepartmentRepository departmentRepo , 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_employeeRepo = employeeRepository;
            //_departmentRepo = departmentRepo;
            _mapper = mapper;
        }
        public IActionResult Index(string? searchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(searchInput))
            {
                 employees = _unitOfWork.EmployeeRepository.GetAll();

            }

            else
            {
                 employees = _unitOfWork.EmployeeRepository.GetByName(searchInput);
            }
            // Dictionary : 
            // Storage => ViewData , ViewBag , TempData

            // 1. ViewData : transfer extra info from action to view
            // 2. ViewBag : transfer extra info from action to view &&  Safety Type ==> 'Generic'
            // 3. TempData : transfer extra info from one request to another

            //var employees = _employeeRepo.GetAll();
            ViewData["Message"] = "Hello from ViewData";
            ViewBag.Message2 = "Hello from ViewBag";
            ViewBag.NewType = new { message = "Anoynomous Type" };
            return View(employees);


        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if(ModelState.IsValid )
            {

                try
                {
                   if (model.Image is not null)
                    {
                        model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                    }

                   var employee= _mapper.Map<Employee>(model);

                     _unitOfWork.EmployeeRepository.Add(employee);
                    var count = _unitOfWork.Complete();
                    if (count > 0)
                    {
                        TempData["Message"] = "Employee is Created Success";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
               
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Details(int? id , string viewname = "Details")
        {
            if (id is null) return BadRequest();
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value); 

            if (employee == null) return NotFound(new { statusCode = 400, messege = $"Employee With Id:{id} is Not Found" });


            return View(viewname, employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;

            if (id is null) return BadRequest();
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (employee == null) return NotFound(new { statusCode = 400, messege = $"Employee With Id:{id} is Not Found" });
            
            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(employeeDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var departments = _unitOfWork.DepartmentRepository.GetAll();
                ViewData["departments"] = departments;

                var employee = _unitOfWork.EmployeeRepository.Get(id);
                if (employee == null) return NotFound(new { statusCode = 400, messege = $"Employee With Id:{id} is Not Found" });

                _mapper.Map(model, employee);

                _unitOfWork.EmployeeRepository.Update(employee);
                var count = _unitOfWork.Complete();

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
                var employee = _unitOfWork.EmployeeRepository.Get(id);

                if (employee == null) return NotFound(new { statusCode = 400, messege = $"Employee With Id:{id} is Not Found" });

                
                _unitOfWork.EmployeeRepository.Delete(employee);
                var count = _unitOfWork.Complete();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                
            }
            return View(model);
        }



    }
}
