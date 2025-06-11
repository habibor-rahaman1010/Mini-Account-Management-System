using Account.Management.Domain;
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
        private IApplicationTime _applicationTime;

        public VoucherController(IVoucherManagementService voucherManagementService, 
            IConfiguration configuration,
            IApplicationTime applicationTime,
            IMapper mapper)
        {
            _voucherManagementService = voucherManagementService;
            _configuration = configuration;
            _applicationTime = applicationTime;
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

        public async Task<IActionResult> EditVoucher(Guid id)
        {
            var existVoucher = await _voucherManagementService.GetVoucherById("READBYID", id);
            if (existVoucher == null)
            {
                return NotFound("Voucher not found!");
            }

            var updateModel = _mapper.Map<VoucherUpdateModel>(existVoucher);
            ViewBag.VoucherTypes = await GetVoucherTypeSelectList(updateModel.VoucherTypeId);
            return View(updateModel);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVoucher(Guid id, VoucherUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.VoucherTypes = await GetVoucherTypeSelectList(model.VoucherTypeId);
                return View(model);
            }

            var existVoucher = await _voucherManagementService.GetVoucherById("READBYID", id);
            if (existVoucher == null)
            {
                return NotFound("Voucher not found!");
            }

            var updateVoucher = _mapper.Map(model, existVoucher);
            updateVoucher.VoucherUpdateAt = _applicationTime.GetCurrentTime();
            await _voucherManagementService.UpdateVoucher("UPDATE", id, updateVoucher);
            return RedirectToAction("VoucherList", "Voucher");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveVoucher(Guid id)
        {
            var existVoucher = await _voucherManagementService.GetVoucherById("READBYID", id);
            if (existVoucher == null)
            {
                return NotFound("The voucher is not found!");
            }
            await _voucherManagementService.DeleteVoucher("DELETE", id);
            return RedirectToAction("VoucherList", "Voucher");
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
                        var id = reader["Id"].ToString();
                        var typeName = reader["TypeName"].ToString();

                        list.Add(new SelectListItem
                        {
                            Value = id,
                            Text = typeName,
                            Selected = selectedValue.HasValue && id == selectedValue.Value.ToString()
                        });
                    }
                }
                await connection.CloseAsync();
            }

            return new SelectList(list, "Value", "Text", selectedValue?.ToString());
        }
    }
}