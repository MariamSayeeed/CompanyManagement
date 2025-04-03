using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;
using Microsoft.AspNetCore.Authorization;

using MVC03.PL.Helpers;
using System.Drawing;

namespace MVC03.PL.Controllers
{
    [Authorize]

    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<AppUser> userManager) 
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToReturnDto> roles;

            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,
                 
                });
            }

            else
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,

                }).Where(R=>R.Name.ToLower().Contains(SearchInput.ToLower()));
            }

            return View(roles);


        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    var role = await _roleManager.FindByNameAsync(model.Name);
                    if (role is null)
                    {
                        role = new IdentityRole()
                        {
                            Name = model.Name,
                        };

                        var result = await _roleManager.CreateAsync(role);

                        if (result.Succeeded)
                        {
                            TempData["Message"] = "Role is Created Success";
                            return RedirectToAction(nameof(Index));
                        }

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
        public async Task<IActionResult> Details(string? id, string viewname = "Details")
        {
            if (id is null) return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null) return NotFound(new { statusCode = 400, messege = $"Role With Id:{id} is Not Found" });

            var dto = new RoleToReturnDto()
            {
                Id = role.Id,
                Name = role.Name,
            };


            return View(viewname, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {


            return await Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                
                if (id != model.Id) return BadRequest("Invalid Opearation");
                var role = await _roleManager.FindByIdAsync(id);

                if (role is null) return NotFound(new { statusCode = 400, messege = $"User With Id:{id} is Not Found" });
                if (await _roleManager.FindByNameAsync(model.Name) is null)
                {

                    role.Name = model.Name;

                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));

                    }

                    ModelState.AddModelError("", "Invalid Opeartion");

                }

                ModelState.AddModelError("", "Invalid Opeartion");

            }

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDto model)
        {


            if (ModelState.IsValid)
            {

                if (id != model.Id) return BadRequest("Invalid Opearation");
                var role = await _roleManager.FindByIdAsync(id);

                if (role is null) return NotFound(new { statusCode = 400, messege = $"User With Id:{id} is Not Found" });
              
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                   return RedirectToAction(nameof(Index));

                }

                 
                ModelState.AddModelError("", "Invalid Opeartion");


            }

            return View(model);


        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return BadRequest("Not Found");

            ViewData["RoleId"] = roleId;

            var usersInRoleList = new List<UsersInRoleDto>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userInRole = new UsersInRoleDto()
                {
                    UserId = user.Id,
                    UserName = user.UserName,

                };

                if (await _userManager.IsInRoleAsync(user, role.Name)) 
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected= false;
                }


                usersInRoleList.Add(userInRole);

            }


            return View(usersInRoleList);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId , List<UsersInRoleDto> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return BadRequest("Not Found");

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var AppUser = await _userManager.FindByIdAsync(user.UserId);
                    if (AppUser is not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(AppUser , role.Name))
                        {
                           await _userManager.AddToRoleAsync(AppUser, role.Name);

                        }
                         else if (!user.IsSelected && await _userManager.IsInRoleAsync(AppUser, role.Name))
                        {
                           await _userManager.RemoveFromRoleAsync(AppUser, role.Name);
                        }
                    }
                   // ModelState.AddModelError("", "Invalid Opearation");

                }

                return RedirectToAction("Edit", new { id = roleId});

            }


            return View(users);
        }

    }
}
