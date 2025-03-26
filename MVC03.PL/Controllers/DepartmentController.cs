using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC03.BLL.Interfaces;
using MVC03.BLL.Repositories;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;


namespace MVC03.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _deptRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController( IUnitOfWork unitOfWork,
            //IDepartmentRepository departmentRepository , 
            IMapper mapper
            )
        {
            //_deptRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]  // GET ://Department//Index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // server side validation
            {
                try
                {
                    var department = new Department()
                    {
                        Name = model.Name,
                        Code = model.Code,
                        CreateAt = model.CreateAt
                    };
                    await _unitOfWork.DepartmentRepository.AddAsync(department);
                    var count = await _unitOfWork.CompleteAsync();

                    if (count > 0)
                    {
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
        public async Task<IActionResult> Details(int? id , string viewname= "Details")
        {
            if (id is null) return BadRequest("Invalid Id ");
           
            var deprtment = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);

            if (deprtment == null) return NotFound(new { statusCode =400 , messege = $"Department With Id:{id} is Not Found" });   

            return View(viewname, deprtment);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id is null) return BadRequest("Invalid Id ");

            var deprtment = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);

            if (deprtment == null) return NotFound(new { statusCode = 400, messege = $"Department With Id:{id} is Not Found" });
            //var departmentDto = new CreateDepartmentDto
            //{
            //    Code = deprtment.Code,
            //    Name = deprtment.Name,

            //};
            var departmentDto = _mapper.Map<CreateDepartmentDto> (deprtment);

            return View(departmentDto);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id,Department department)
        //{
        //    if (ModelState.IsValid) // server side validation
        //    {
        //        if (id == department.Id)   // ---> defensive code 
        //        {
        //            var count = _deptRepository.Update(department);
        //            if (count > 0)
        //            {
        //                return RedirectToAction(nameof(Index));
        //            }
        //        }

        //    }

        //    return View(department);
        //}


        [HttpPost]    //-----> Another way for Edit 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id,CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // server side validation
            {
                var department = await _unitOfWork.DepartmentRepository.GetAsync(id);

                if (department == null) return NotFound(new { statusCode = 400, messege = $"Department With Id:{id} is Not Found" });

                //department.Name = model.Name;
                //department.Code = model.Code;
                //department.CreateAt = model.CreateAt;
                _mapper.Map(model, department);

               _unitOfWork.DepartmentRepository.Update(department);
                var count = await _unitOfWork.CompleteAsync();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest("Invalid Id ");

            var deprtment = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);

            if (deprtment == null) return NotFound(new { statusCode = 400, messege = $"Department With Id:{id} is Not Found" });
            _unitOfWork.DepartmentRepository.Delete(deprtment);
            
            var count = await _unitOfWork.CompleteAsync();

            if (count > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }


    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute]int id,Department model)
        {
            if (ModelState.IsValid) // server side validation
            {
                var department = await _unitOfWork.DepartmentRepository.GetAsync(id);

                if (department == null) return NotFound(new { statusCode = 400, messege = $"Department With Id:{id} is Not Found" });

                _unitOfWork.DepartmentRepository.Delete(department);
                var count = await _unitOfWork.CompleteAsync();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

    }
}
