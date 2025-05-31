using Account.Management.Domain;
using Account.Management.Infrastructure.Account.Management.Identity;
using Account.Management.Infrastructure.Extentions;
using Account.Management.Web.Areas.Admin.Models;
using Account.Management.Web.Utitlity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Account.Management.Web.Areas.Admin.Controllers.AccountManagement
{
    [Area("Admin")]
    public class AccountManagementController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IApplicationTime _applicationTime;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountManagementController> _logger;

        public AccountManagementController(RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IApplicationTime applicationTime,
            IMapper mapper,
            ILogger<AccountManagementController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationTime = applicationTime;
            _mapper = mapper;
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

        //This mehtod get a user by id for update...
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return Json(new { success = false });
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            return Json(new
            {
                success = true,
                data = new
                {
                    id = user.Id,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email,
                    phoneNumber = user.PhoneNumber,
                    address = user.Address,
                    userRoles = userRoles,       // Current roles
                    availableRoles = allRoles    // All available roles
                }
            });
        }

        //This is user update mehtod also user roles update code...
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserUpdateModel model, List<string> Roles)
        {
            if (ModelState.IsValid)
            {
                // Get the user
                var user = await _userManager.FindByIdAsync(model.Id.ToString());
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }

                // Validate the roles to ensure they exist
                var validRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                var invalidRoles = Roles.Where(r => !validRoles.Contains(r)).ToList();

                if (invalidRoles.Any())
                {
                    return Json(new { success = false, message = $"The following roles do not exist: {string.Join(", ", invalidRoles)}" });
                }

                // Update user properties
                user = _mapper.Map(model, user);

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return Json(new { success = false, message = "Failed to update user." });
                }

                // Get the current roles of the user
                var currentRoles = await _userManager.GetRolesAsync(user);

                // Determine roles to remove and add
                var rolesToRemove = currentRoles.Except(Roles).ToList();
                var rolesToAdd = Roles.Except(currentRoles).ToList();

                // Remove roles the user no longer has
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    return Json(new { success = false, message = "Failed to remove user roles." });
                }

                // Add new roles the user is assigned
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                {
                    return Json(new { success = false, message = "Failed to add new roles." });
                }

                return Json(new { success = true, message = "User updated successfully." });
            }
            return Json(new { success = false, message = "Invalid data." });
        }


        //This is delete user method...
        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user != null)
                {
                    // Remove user roles
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Any())
                    {
                        await _userManager.RemoveFromRolesAsync(user, roles);
                    }

                    var claims = await _userManager.GetClaimsAsync(user);
                    if (claims.Any())
                    {
                        await _userManager.RemoveClaimsAsync(user, claims);
                    }

                    var result = await _userManager.DeleteAsync(user);

                    if (result.Succeeded)
                    {
                        // Check if the deleted user is the currently logged-in user
                        var currentUserId = _userManager.GetUserId(User);
                        if (currentUserId == user.Id.ToString())
                        {
                            await _signInManager.SignOutAsync();
                        }

                        return Json(new
                        {
                            success = true,
                            message = "The User has been deleted successfully."
                        });
                    }
                }

                return Json(new
                {
                    success = true,
                    message = "The User has deleted successfuly"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The User deleted failed");
                return Json(new
                {
                    success = true,
                    message = "The User deleted failed"
                });
            }
        }

        public IActionResult RoleList(int page = 1, int pageSize = 10)
        {
            var rolesQuery = _roleManager.Roles.AsQueryable();
            int totalRoles = rolesQuery.Count();

            var pager = new Pager(totalRoles, page, pageSize);

            var roles = rolesQuery
                .Skip(pager.StartIndex)
                .Take(pager.PageSize)
                .ToList();

            var model = new RoleListViewModel
            {
                Roles = roles,
                Pager = pager
            };

            return View(model);
        }

        
        public async Task<IActionResult> EditRole(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound();
            }
            return View(new RoleUpdateModel { Name = role.Name});
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(Guid id, RoleUpdateModel model)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound();
            }
            role.Name = model.Name;
            role.NormalizedName = model.Name.ToUpper();
            role.ConcurrencyStamp = _applicationTime.GetCurrentTime().Ticks.ToString();
            
            var result = await _roleManager.UpdateAsync(role);
            if(result.Succeeded)
            {
                return RedirectToAction("RoleList", "AccountManagement");
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if(role == null)
            {
                return NotFound();
            }
            await _roleManager.DeleteAsync(role);

            return RedirectToAction("RoleList", "AccountManagement");
        }

        //User Role Change method...
        public IActionResult ChangeRole()
        {
            var model = new RoleChangeModel();
            LoadValues(model);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(RoleChangeModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                var newRole = await _roleManager.FindByIdAsync(model.RoleId.ToString());
                await _userManager.AddToRoleAsync(user, newRole.Name);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = $"The user old role changed and assigned to new role!",
                    Type = ResponseTypes.Success
                });
                return RedirectToAction("GetUserList", "AccountManagement");
            }
            LoadValues(model);
            TempData.Put("ResponseMessage", new ResponseModel
            {
                Message = $"The user role was not changed!",
                Type = ResponseTypes.Danger
            });
            return View(model);
        }

        private void LoadValues(RoleChangeModel model)
        {
            // Get users and roles as lists and insert the default "Select" option at the beginning
            var usersList = _userManager.Users
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Email })
                .ToList();
            usersList.Insert(0, new SelectListItem { Value = "", Text = "--Select A User--" });

            var rolesList = _roleManager.Roles
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();
            rolesList.Insert(0, new SelectListItem { Value = "", Text = "--Select A Role--" });

            // Assign to model
            model.Users = new List<SelectListItem>(usersList);
            model.Roles = new List<SelectListItem>(rolesList);
        }
    }
}
