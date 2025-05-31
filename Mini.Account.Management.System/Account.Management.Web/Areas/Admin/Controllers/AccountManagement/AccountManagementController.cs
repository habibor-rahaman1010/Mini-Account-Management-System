using Account.Management.Domain;
using Account.Management.Infrastructure.Account.Management.Identity;
using Account.Management.Infrastructure.Extentions;
using Account.Management.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Account.Management.Web.Areas.Admin.Controllers.AccountManagement
{
    [Area("Admin")]
    public class AccountManagementController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationTime _applicationTime;
        private readonly ILogger<AccountManagementController> _logger;

        public AccountManagementController(RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IApplicationTime applicationTime,
            ILogger<AccountManagementController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _applicationTime = applicationTime;
            _logger = logger;
        }

        public IActionResult CreateRole()
        {          
            return View();
        }

        private async Task<bool> IsRoleHas(string roleName)
        {
           return await _roleManager.RoleExistsAsync(roleName);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new ApplicationRole()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    NormalizedName = model.Name.ToUpper(),
                    ConcurrencyStamp = _applicationTime.GetCurrentTime().Ticks.ToString(),
                };

                if (IsRoleHas(role.Name).Result == true)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = $"The Role '{model.Name}' already have in the system!",
                        Type = ResponseTypes.Warning
                    });

                    ModelState.Clear();
                    return View();
                }

                var result = await _roleManager.CreateAsync(role);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "The Role has been created successfuly!",
                    Type = ResponseTypes.Success
                });

                if (result.Succeeded)
                {
                    ModelState.Clear();
                    return View();
                }
            }        
            return View(model);
        }


        
    }
}
