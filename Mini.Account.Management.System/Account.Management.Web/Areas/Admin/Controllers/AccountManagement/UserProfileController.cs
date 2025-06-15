using Account.Management.Infrastructure.Account.Management.Identity;
using Account.Management.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Account.Management.Web.Areas.Admin.Controllers.AccountManagement
{
    [Area("Admin"), Authorize]
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin, Accountant, Viewer")]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(currentUser);

            var userViewModel = new UserProfileViewModel
            {
                User = currentUser,
                Roles = roles
            };

            return View(userViewModel);
        }
    }
}
