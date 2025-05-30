using Microsoft.AspNetCore.Mvc;

namespace Account.Management.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashbordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
