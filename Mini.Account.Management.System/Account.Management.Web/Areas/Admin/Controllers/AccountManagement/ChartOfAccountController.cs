using Account.Management.Web.Utitlity;
using Account.Management.Domain.Entities;
using Account.Management.Web.Areas.Admin.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Account.Management.Domain.Dtos;
using Account.Management.Domain.ServicesInterface;

namespace Account.Management.Web.Areas.Admin.Controllers.AccountManagement
{
    [Area("Admin"), Authorize]
    public class ChartOfAccountController : Controller
    {
        private readonly IChartOfAccountManagementService _chartOfAccountManagementService;
        private readonly IMapper _mapper;
        public ChartOfAccountController(IChartOfAccountManagementService chartOfAccountManagementService,
            IMapper mapper)
        {
            _chartOfAccountManagementService = chartOfAccountManagementService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddChartOfAccount()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddChartOfAccount(ChartOfAccountModel model)
        {
            if (ModelState.IsValid)
            {
                var account = _mapper.Map<ChartOfAccount>(model);
                account.Id = Guid.NewGuid();
                await _chartOfAccountManagementService.CreateChatOfAccount("CREATE", account);
                return RedirectToAction("ChartOfAccountList", "ChartOfAccount");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Accountant, Viewer")]
        public async Task<IActionResult> ChartOfAccountList(int page = 1, int pageSize = 10)
        {
            var (chartOfAccounts, totalCount) = await _chartOfAccountManagementService.GetchatOfAccounts("READ", page, pageSize);
            var pager = new Pager(totalCount, page, pageSize);

            if (chartOfAccounts == null || chartOfAccounts.Count() <= 0)
            {
                ViewBag.Message = "No data available";
                chartOfAccounts = new List<ChartOfAccount>();
            }

            var model = new ChartOfAccountListViewModel
            {
                ChartOfAccounts = _mapper.Map<IList<ChartOfAccountDto>>(chartOfAccounts),
                Pager = pager
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditChartOfAccount(Guid id)
        {
            var account = await _chartOfAccountManagementService.ChartOfAccountById("READBYID", id);
            var accountUpdateDto = _mapper.Map<ChartOfAccountUpdateDto>(account);
            return View(accountUpdateDto);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditChartOfAccount(Guid id, ChartOfAccountUpdate model)
        {
            if (ModelState.IsValid)
            {
                var account = _mapper.Map<ChartOfAccount>(model);
                await _chartOfAccountManagementService.UpdateChatOfAccount("UPDATE", id, account);
                return RedirectToAction("ChartOfAccountList", "ChartOfAccount");
            }
            return View(_mapper.Map<ChartOfAccountUpdateDto>(model));
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveChartOfAccount(Guid id)
        {
            await _chartOfAccountManagementService.DeleteChartOfAccount("DELETE", id);
            return RedirectToAction("ChartOfAccountList", "ChartOfAccount");
        }
    }
}
