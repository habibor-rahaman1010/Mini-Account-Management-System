using Account.Management.Application.Services;
using Account.Management.Domain.Dtos;
using Account.Management.Domain.Entities;
using Account.Management.Domain.ServicesInterface;
using Account.Management.Web.Areas.Admin.Models;
using Account.Management.Web.Utitlity;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace Account.Management.Web.Areas.Admin.Controllers.VoucherEntryModules
{
    [Area("Admin")]
    public class VoucherController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        private readonly IVoucherManagementService _voucherManagementService;
        private readonly IMapper _mapper;

        public VoucherController(IVoucherManagementService voucherManagementService, 
            IConfiguration configuration,
            IMapper mapper)
        {
            _voucherManagementService = voucherManagementService;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
        }

        public async Task<IActionResult> AddVoucher()
        {
            ViewBag.VoucherTypes = await GetVoucherTypeSelectList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVoucher(VoucherModel model)
        {
            if (ModelState.IsValid)
            {
                var voucherInput = _mapper.Map<Voucher>(model);
                voucherInput.Id = Guid.NewGuid();
                await _voucherManagementService.CreateVoucher("CREATE", voucherInput);
                return RedirectToAction("VoucherList", "Voucher");
            }
            return View(model);
        }

        public async Task<IActionResult> VoucherList(int pageNumber = 1, int pageSize = 10)
        {
            var (vouchers, totalCount) = await _voucherManagementService.GetVouchers("READ", pageNumber, pageSize);

            if (vouchers == null || vouchers.Count() <= 0)
            {
                ViewBag.Message = "No data available";
                vouchers = new List<Voucher>();
            }

            var pager = new Pager(totalCount, pageNumber, pageSize);

            var model = new VoucherListViewModel()
            {
                Vouchers = _mapper.Map<IList<VoucherDto>>(vouchers),
                Pager = pager
            };
            return View(model);
        }

        private async Task<SelectList> GetVoucherTypeSelectList()
        {
            var list = new List<SelectListItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand("SELECT Id, TypeName FROM VoucherTypes", connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        list.Add(new SelectListItem
                        {
                            Value = reader["Id"].ToString(),
                            Text = reader["TypeName"].ToString()
                        });
                    }
                }
                await connection.CloseAsync();
            }
            return new SelectList(list, "Value", "Text");
        }
    }
}
