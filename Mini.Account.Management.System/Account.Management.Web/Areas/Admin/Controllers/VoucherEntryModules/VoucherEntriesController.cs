using Account.Management.Domain.ServicesInterface;
using Account.Management.Domain;
using Account.Management.Web.Areas.Admin.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Account.Management.Domain.Entities;

namespace Account.Management.Web.Areas.Admin.Controllers.VoucherEntryModules
{
    [Area("Admin")]
    public class VoucherEntriesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        private readonly IVoucherEntriesManagementService _voucherEntriesManagementService;
        private readonly IMapper _mapper;
        private IApplicationTime _applicationTime;

        public VoucherEntriesController(IVoucherEntriesManagementService voucherEntriesManagementService,
            IConfiguration configuration,
            IApplicationTime applicationTime,
            IMapper mapper)
        {
            _voucherEntriesManagementService = voucherEntriesManagementService;
            _configuration = configuration;
            _applicationTime = applicationTime;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
        }

        public async Task<IActionResult> AddVoucherEntry()
        {
            ViewBag.ChatOfAccounts = await GetAccountSelectList();
            ViewBag.VoucherTypes = await GetVoucherTypeSelectList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVoucherEntry(VoucherEntryCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var voucherEntry = _mapper.Map<VoucherEntry>(model);
                voucherEntry.Id = Guid.NewGuid();
                await _voucherEntriesManagementService.CreateVoucher("CREATE", voucherEntry);
                return RedirectToAction("VoucherList", "Voucher");
            }
            return View(model);
        }









        private async Task<SelectList> GetAccountSelectList(Guid? selectedValue = null)
        {
            var list = new List<SelectListItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("SELECT Id, AccountName FROM ChartOfAccounts", connection))
            {
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var id = reader.GetGuid(reader.GetOrdinal("Id"));
                        var name = reader.GetString(reader.GetOrdinal("AccountName"));

                        list.Add(new SelectListItem
                        {
                            Value = id.ToString(),
                            Text = name,
                            Selected = selectedValue.HasValue && id == selectedValue.Value
                        });
                    }
                }
            }

            return new SelectList(list, "Value", "Text", selectedValue?.ToString());
        }

        private async Task<SelectList> GetVoucherTypeSelectList(Guid? selectedValue = null)
        {
            var list = new List<SelectListItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("SELECT Id, TypeName FROM VoucherTypes", connection))
            {
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var id = reader.GetGuid(reader.GetOrdinal("Id"));
                        var typeName = reader.GetString(reader.GetOrdinal("TypeName"));

                        list.Add(new SelectListItem
                        {
                            Value = id.ToString(),
                            Text = typeName,
                            Selected = selectedValue.HasValue && id == selectedValue.Value
                        });
                    }
                }
            }

            return new SelectList(list, "Value", "Text", selectedValue?.ToString());
        }
    }
}
