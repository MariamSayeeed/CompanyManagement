using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVC03.PL.Controllers
{


    //  P@sW0rrd
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        #region Sign Up
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpDto model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user is null)
                {
                    user =await _userManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                         user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsArgee = model.IsAgree
                        };


                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {

                            return RedirectToAction("SignIn");
                        }

                        foreach (var error in result.Errors)
                            ModelState.AddModelError("", error.Description);
                    }
                }

                ModelState.AddModelError("", "Invalid SignUp !!");

            }

            return View();
        }

        #endregion


        #region Sign In

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(SignInDto model)
        {

            return View();
        }



        #endregion


        #region Sign Out

        #endregion



    }
}
