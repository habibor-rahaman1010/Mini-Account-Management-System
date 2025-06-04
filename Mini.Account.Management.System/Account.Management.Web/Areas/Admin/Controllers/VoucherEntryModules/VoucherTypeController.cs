using Account.Management.Domain.Dtos;
using Account.Management.Domain.Entities;
using Account.Management.Domain.ServicesInterface;
using Account.Management.Web.Areas.Admin.Models;
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

        public async Task<IActionResult> VoucherTypeList(int pageNumber = 1, int pageSize = 2)
        {
            var(voucherTypes, totalCount)  = await _voucherTypeManagementService.GetVoucherTypes("READ", pageNumber, pageSize);

            if (voucherTypes == null)
            {
                ViewBag.Message = "No voucher types found.";
                voucherTypes = new List<VoucherType>();
            }

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalCount = totalCount;    
            var voucherTypeDto = _mapper.Map<IList<VoucherTypeDto>>(voucherTypes);
            return View(voucherTypeDto);
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
                return View(model);
            }
            return View(model);
        }


    }
}
