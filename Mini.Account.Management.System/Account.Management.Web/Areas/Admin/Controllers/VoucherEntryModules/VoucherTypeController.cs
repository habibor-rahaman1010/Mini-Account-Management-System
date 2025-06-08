using Account.Management.Domain.Dtos;
using Account.Management.Domain.Entities;
using Account.Management.Domain.ServicesInterface;
using Account.Management.Web.Areas.Admin.Models;
using Account.Management.Web.Utitlity;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Account.Management.Web.Areas.Admin.Controllers.VoucherEntryModules
{
    [Area("Admin")]
    public class VoucherTypeController : Controller
    {
        private readonly IVoucherTypeManagementService _voucherTypeManagementService;
        private readonly IMapper _mapper;
        public VoucherTypeController(IVoucherTypeManagementService voucherTypeManagementService, IMapper mapper)
        {
            _voucherTypeManagementService = voucherTypeManagementService;
            _mapper = mapper;
        }

        public async Task<IActionResult> VoucherTypeList(int pageNumber = 1, int pageSize = 10)
        {
            var(voucherTypes, totalCount)  = await _voucherTypeManagementService.GetVoucherTypes("READ", pageNumber, pageSize);

            if (voucherTypes == null)
            {
                ViewBag.Message = "No voucher types found.";
                voucherTypes = new List<VoucherType>();
            }

            var pager = new Pager(totalCount, pageNumber, pageSize);

            var model = new VoucherTypeListViewModel()
            {
                VoucherTypes = _mapper.Map<IList<VoucherTypeDto>>(voucherTypes),
                Pager = pager
            };
            return View(model);
        }

        public IActionResult CreateVoucherType()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVoucherType(VoucherTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var voucherType = _mapper.Map<VoucherType>(model);
                voucherType.Id = Guid.NewGuid();
                await _voucherTypeManagementService.AddVoucherType("CREATE", voucherType);
                return RedirectToAction("VoucherTypeList", "VoucherType");
            }
            return View(model);
        }

        public async Task<IActionResult> EditVoucherType(Guid id)
        {
            var existVoucherType = await _voucherTypeManagementService.GetVoucherTypeById("READBYID", id);
            if (existVoucherType == null)
            {
                return NotFound("The voucher type not found");
            }
            return View(_mapper.Map<VoucherUpdateModel>(existVoucherType));
        }

        [HttpPost]
        public async Task<IActionResult> EditVoucherType(Guid id, VoucherUpdateModel model)
        {
            var existVoucherType = await _voucherTypeManagementService.GetVoucherTypeById("READBYID", id);
            if (existVoucherType == null)
            {
                return NotFound("The voucher type not found");
            }
            var voucherType = _mapper.Map(model, existVoucherType);
            await _voucherTypeManagementService.UpdateVoucherType("UPDATE", id, voucherType);
            return RedirectToAction("VoucherTypeList", "VoucherType");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _voucherTypeManagementService.DeleteVoucherType("DELETE", id);
            return RedirectToAction("VoucherTypeList", "VoucherType");
        }
    }
}
