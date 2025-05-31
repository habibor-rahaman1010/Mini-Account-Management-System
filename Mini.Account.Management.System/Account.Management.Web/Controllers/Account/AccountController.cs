using Account.Management.Infrastructure.Account.Management.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Account.Management.Web.Models.AccoutModels;
using Microsoft.AspNetCore.Authentication;
using Account.Management.Infrastructure.Extentions;
using Account.Management.Web.Areas.Admin.Models;

namespace Account.Management.Web.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        //--------Registration Code-----------
        public async Task<IActionResult> RegisterAsync(string returnUrl = null)
        {
            var model = new RegistrationModel();
            model.ReturnUrl = returnUrl;
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(RegistrationModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FistName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email,
                };
                if (await UserAlreadyExist(user.Email) == true)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = $"The Email : '{user.Email}' alredy exist try again by another email!",
                        Type = ResponseTypes.Danger
                    });
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");
                        await _userManager.AddToRoleAsync(user, "Viewer");

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "The account has been created successfuly!",
                            Type = ResponseTypes.Success
                        });
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }


        //--------Login Code-----------
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(string returnUrl = null)
        {
            var model = new SigninModel();

            if (!string.IsNullOrEmpty(model.ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, model.ErrorMessage);
            }

            model.ReturnUrl = returnUrl == null ? Url.Content("~/") : returnUrl;

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost, AutoValidateAntiforgeryToken, AllowAnonymous]
        public async Task<IActionResult> LoginAsync(SigninModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "The user loged in successfuly!",
                        Type = ResponseTypes.Success
                    });
                    return LocalRedirect(model.ReturnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("./LoginWith2fa", new { ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "The user can not login somthing happed wrong!",
                        Type = ResponseTypes.Danger
                    });
                    return View(model);
                }
            }
            return View(model);
        }


        //--------Logout Code-----------
        [AllowAnonymous]
        public async Task<IActionResult> LogoutAsync(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            returnUrl ??= Url.Content("~/");
            TempData.Put("ResponseMessage", new ResponseModel
            {
                Message = "The user loged out successfuly!",
                Type = ResponseTypes.Success
            });
            return LocalRedirect(returnUrl);
        }

        private async Task<bool> UserAlreadyExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            return true;
        }
    }
}
