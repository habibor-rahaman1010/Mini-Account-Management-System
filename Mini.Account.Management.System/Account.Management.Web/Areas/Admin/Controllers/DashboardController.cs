using Account.Management.Domain.ServicesInterface;
using Account.Management.Infrastructure.Account.Management.Identity;
using Account.Management.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Account.Management.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IChartOfAccountManagementService _chartOfAccountManagementService;
        private readonly IVoucherTypeManagementService _voucherTypeManagementService;
        private readonly IVoucherManagementService _voucherManagementService;
        private readonly IVoucherEntriesManagementService _voucherEntriesManagementService;

        public DashboardController(IChartOfAccountManagementService chartOfAccountManagementService,
            IVoucherTypeManagementService voucherTypeManagementService,
            IVoucherManagementService voucherManagementService,
            IVoucherEntriesManagementService voucherEntriesManagementService,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _chartOfAccountManagementService = chartOfAccountManagementService;
            _voucherTypeManagementService = voucherTypeManagementService;
            _voucherManagementService = voucherManagementService;
            _voucherEntriesManagementService = voucherEntriesManagementService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardItemCount = new DashboardItemCountModel
            {
                TotalUser = _userManager.Users.Count(),
                TotalRole = _roleManager.Roles.Count(),
                TotalChartOfAccount = await _chartOfAccountManagementService.GetTotalChartOfAccountsCountAsync("COUNT"),
                TotalVoucherType = await _voucherTypeManagementService.GetTotalVoucherTypesCountAsync("COUNT"),
                TotalVoucher = await _voucherManagementService.GetTotalVouchersCountAsync("COUNT"),
                TotalVoucherEntry = await _voucherEntriesManagementService.GetTotalVoucherEntriesCountAsync("COUNT")
            };

            return View(dashboardItemCount);
        }
    }
}
