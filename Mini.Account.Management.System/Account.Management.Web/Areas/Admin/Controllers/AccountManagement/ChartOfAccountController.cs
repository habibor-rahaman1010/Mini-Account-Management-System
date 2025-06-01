using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Account.Management.Web.Areas.Admin.Controllers.AccountManagement
{
    [Area("Admin"), Authorize]
    public class ChartOfAccountController : Controller
    {
        public IActionResult AddChartOfAccount()
        {
            return View();
        }
    }
}
