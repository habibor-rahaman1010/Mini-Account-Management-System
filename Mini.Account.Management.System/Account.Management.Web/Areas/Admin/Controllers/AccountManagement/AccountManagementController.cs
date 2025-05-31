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

        //Get all user list...
        public IActionResult GetUserList()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetUserList(int draw, int start, int length, string search, List<Order> order)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.UserName.Contains(search) || u.Email.Contains(search));
            }

            var filteredCount = query.Count();

            var usersList = query
                .Skip(start)
                .Take(length)
                .ToList();

            var usersWithRoles = new List<(ApplicationUser User, string RoleNames)>();

            foreach (var user in usersList)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleNames = string.Join(", ", roles);
                usersWithRoles.Add((user, roleNames));
            }

            if (order != null && order.Count > 0)
            {
                var columnIndex = order[0].Column;
                var sortDirection = order[0].Direction;

                switch (columnIndex)
                {
                    case 0:
                        usersWithRoles = sortDirection == "asc"
                            ? usersWithRoles.OrderBy(u => u.User.UserName).ToList()
                            : usersWithRoles.OrderByDescending(u => u.User.UserName).ToList();
                        break;

                    case 1:
                        usersWithRoles = sortDirection == "asc"
                            ? usersWithRoles.OrderBy(u => u.User.Email).ToList()
                            : usersWithRoles.OrderByDescending(u => u.User.Email).ToList();
                        break;

                    case 2:
                        usersWithRoles = sortDirection == "asc"
                            ? usersWithRoles.OrderBy(u => u.User.EmailConfirmed).ToList()
                            : usersWithRoles.OrderByDescending(u => u.User.EmailConfirmed).ToList();
                        break;

                    case 3:
                        usersWithRoles = sortDirection == "asc"
                            ? usersWithRoles.OrderBy(u => u.RoleNames).ToList()
                            : usersWithRoles.OrderByDescending(u => u.RoleNames).ToList();
                        break;

                    default:
                        break;
                }
            }

            var data = usersWithRoles.Select(u => new
            {
                u.User.UserName,
                u.User.Email,
                u.User.Id,
                u.User.EmailConfirmed,
                RoleNames = u.RoleNames
            }).ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = _userManager.Users.Count(),
                recordsFiltered = filteredCount,
                data = data
            });
        }
    }
}
