using Account.Management.Web.Utitlity;
using Account.Management.Domain.Entities;
using Account.Management.Domain.RepositoriesInterface;
using Account.Management.Web.Areas.Admin.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Account.Management.Domain.Dtos;

namespace Account.Management.Web.Areas.Admin.Controllers.AccountManagement
{
    [Area("Admin"), Authorize]
    public class ChartOfAccountController : Controller
    {
        private readonly IChartOfAccountRepository _chartOfAccountRepository;
        private readonly IMapper _mapper;
        public ChartOfAccountController(IChartOfAccountRepository chartOfAccountRepository,
            IMapper mapper)
        {
            _chartOfAccountRepository = chartOfAccountRepository;
            _mapper = mapper;
        }

        public IActionResult AddChartOfAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddChartOfAccount(ChartOfAccountModel model)
        {
            if (ModelState.IsValid)
            {
                var account = _mapper.Map<ChartOfAccount>(model);
                account.Id = Guid.NewGuid();
                await _chartOfAccountRepository.CreateAsync("CREATE", account);
            }
            return RedirectToAction("ChartOfAccountList", "ChartOfAccount");
        }

        public async Task<IActionResult> ChartOfAccountList(int page = 1, int pageSize = 10)
        {
            var accounts = await _chartOfAccountRepository.GetChartOfAccountsAsync(page, pageSize);
            var pager = new Pager(accounts.totalCount, page, pageSize);

            var model = new ChartOfAccountListViewModel
            {
                chartOfAccountDtos = accounts.accounts,
                Pager = pager
            };
            return View(model);
        }

        public async Task<IActionResult> UpdateChartOfAccount(Guid id)
        {
            var account = await _chartOfAccountRepository.GetById(id);
            var accountUpdateDto = _mapper.Map<ChartOfAccountUpdateDto>(account);
            return View(accountUpdateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateChartOfAccount(Guid id, ChartOfAccountUpdate model)
        {
            if (ModelState.IsValid)
            {
                var account = _mapper.Map<ChartOfAccountUpdateDto>(model);
                await _chartOfAccountRepository.UpdateAsync(id, account);
                return RedirectToAction("ChartOfAccountList", "ChartOfAccount");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteChartOfAccount(Guid id)
        {
            await _chartOfAccountRepository.DeleteAsync("DELETE", id);
            return RedirectToAction("ChartOfAccountList", "ChartOfAccount");
        }
    }
}
