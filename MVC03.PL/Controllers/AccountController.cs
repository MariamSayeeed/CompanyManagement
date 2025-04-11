
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;
using MVC03.PL.Helpers;
using System.Security.Claims;

namespace MVC03.PL.Controllers
{

    //  P@sW0rrd font-size: .9em;


    // P@SSw0rddd
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly ITwilioService _twilioService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService, ITwilioService twilioService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _twilioService = twilioService;
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
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                        user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree
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
        public async Task<ActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        // Sign in
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }

                    }

                    ModelState.AddModelError("", "Invalid Login !!");
                }

                ModelState.AddModelError("", "Invalid Login !!");

            }

            return View();
        }



        #endregion


        #region Sign Out

        [HttpGet]
        public new async Task<ActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));

        }

        #endregion


        #region Forget Password

        [HttpGet]
        public IActionResult ForgetPasswordUsingEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordURL(ForgetPasswordDto model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    // Generate Token 
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Create URL 
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    // Create obj from Email Class -> Email

                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };
                    // Send Email

                    var flag = EmailSettings.SendEmail(email);
                    if (flag)
                    {
                        return RedirectToAction("CheckYourEmail");
                    }


                }


            }

            ModelState.AddModelError("", "Invalid Reset Password Operation !!");
            return View("ForgetPasswordUsingEmail", model);
        }


        [HttpGet]
        public IActionResult CheckYourEmail()
        {
            return View();
        }

        #endregion

        [HttpGet]
        public IActionResult ConfirmForgetPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgetPasswordUsingSMS()
        {
            return View();
        }




        #region Reset Password

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                if (email is null || token is null) return BadRequest("Invalid Opeartion");
                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {

                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                    ModelState.AddModelError("", "Invalid Operation");


                }
                ModelState.AddModelError("", "Invalid Operation");

            }

            return View();
        }



        #endregion

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordSMS(ForgetPasswordDto model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    // Generate Token 
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Create URL 
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    // Create obj from Email Class -> Email

                    var sms = new SMS()
                    {
                        To = user.PhoneNumber,

                        Body = url
                    };
                    // Send Email

                    _twilioService.SendSMS(sms);

                    return RedirectToAction("CheckYourPhone");

                }


            }

            ModelState.AddModelError("", "Invalid Reset Password Operation !!");
            return View("ForgetPasswordSMS", model);
        }

        [HttpGet]

        public IActionResult CheckYourPhone()
        {
            return View();
        }


        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

            if (!result.Succeeded)
                return RedirectToAction("SignIn", "Auth");

            var externalUser = result.Principal;

            var email = externalUser.FindFirstValue(ClaimTypes.Email);
            var givenName = externalUser.FindFirstValue(ClaimTypes.GivenName);
            var surname = externalUser.FindFirstValue(ClaimTypes.Surname);
            var name = externalUser.FindFirstValue(ClaimTypes.Name);

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = $"{givenName}{surname}".Replace(" ", ""),  
                    Email = email,  
                    EmailConfirmed = true,
                    FirstName = givenName ?? name?.Split(' ').FirstOrDefault() ?? "GoogleUser",
                    LastName = surname ?? (name != null && name.Split(' ').Length > 1 ? name.Split(' ').Last() : "User")
                };

                var createResult = await _userManager.CreateAsync(user);

                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("SignIn");
                }
            }


            ViewData["FirstName"] = givenName;


            await _signInManager.SignInAsync(user, isPersistent: false);


            return RedirectToAction("Index", "Home");
        }


        public IActionResult FacebookLogin()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("FacebookResponse")
            };
            return Challenge(prop, FacebookDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (!result.Succeeded)
                return RedirectToAction("SignIn", "Account");

            var externalUser = result.Principal;
            var email = externalUser.FindFirstValue(ClaimTypes.Email);
            var givenName = externalUser.FindFirstValue(ClaimTypes.GivenName);
            var surname = externalUser.FindFirstValue(ClaimTypes.Surname);
            var name = externalUser.FindFirstValue(ClaimTypes.Name);

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = givenName ?? name?.Split(' ').FirstOrDefault() ?? "FacebookUser",
                    LastName = surname ?? (name != null && name.Split(' ').Length > 1 ? name.Split(' ').Last() : "User")
                };

                var createResult = await _userManager.CreateAsync(user);

                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("SignIn");
                }
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
