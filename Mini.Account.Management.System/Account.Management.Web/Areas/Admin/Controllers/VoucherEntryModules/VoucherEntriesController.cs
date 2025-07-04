﻿using Account.Management.Domain.ServicesInterface;
using Account.Management.Domain;
using Account.Management.Web.Areas.Admin.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Account.Management.Domain.Entities;
using Account.Management.Domain.Dtos;
using Account.Management.Web.Utitlity;
using Account.Management.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace Account.Management.Web.Areas.Admin.Controllers.VoucherEntryModules
{
    [Area("Admin"), Authorize]
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

        [Authorize(Roles = "Admin, Accountant")]
        public async Task<IActionResult> AddVoucherEntry()
        {
            ViewBag.ChatOfAccounts = await GetAccountSelectList();
            ViewBag.VoucherTypes = await GetVoucherTypeSelectList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin, Accountant")]
        public async Task<IActionResult> AddVoucherEntry(VoucherEntryCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var voucherEntry = _mapper.Map<VoucherEntry>(model);
                voucherEntry.Id = Guid.NewGuid();
                await _voucherEntriesManagementService.CreateVoucherEntry("CREATE", voucherEntry);
                return RedirectToAction("VoucherEntryList", "VoucherEntries");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Accountant, Viewer")]
        public async Task<IActionResult> VoucherEntryList(int pageNumber = 1, int pageSize = 10)
        {
            var (voucherEntries, totalCount) = await _voucherEntriesManagementService.GetVoucherEntries("READ", pageNumber, pageSize);
            if (voucherEntries == null || voucherEntries.Count() <= 0)
            {
                ViewBag.Message = "No data available";
                voucherEntries = new List<VoucherEntry>();
            }

            var pager = new Pager(totalCount, pageNumber, pageSize);

            var model = new VoucherEntryListViewModel()
            {
                VoucherEntries = _mapper.Map<IList<VoucherEntryDto>>(voucherEntries),
                Pager = pager
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditVoucherEntry(Guid id)
        {
            var existingVoucherEntry = await _voucherEntriesManagementService.GetVoucherEntryById("READBYID", id);
            if (existingVoucherEntry == null)
            {
                return NotFound("Voucher entry not found!");
            }
            var updateVoucherEntry = _mapper.Map<VoucherEntryUpdateModel>(existingVoucherEntry);
            ViewBag.ChatOfAccounts = await GetAccountSelectList(updateVoucherEntry.AccountId);
            ViewBag.VoucherTypes = await GetVoucherTypeSelectList(updateVoucherEntry.VoucherTypeId);

            return View(updateVoucherEntry);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditVoucherEntry(Guid id, VoucherEntryUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ChatOfAccounts = await GetAccountSelectList(model.AccountId);
                ViewBag.VoucherTypes = await GetVoucherTypeSelectList(model.VoucherTypeId);
                return View(model);
            }

            var existingVoucherEntry = await _voucherEntriesManagementService.GetVoucherEntryById("READBYID", id);
            if (existingVoucherEntry == null)
            {
                return NotFound("Voucher entry not found!");
            }
            var updateVoucherEntry = _mapper.Map(model, existingVoucherEntry);
            updateVoucherEntry.ModifiedDate = _applicationTime.GetCurrentTime();
            await _voucherEntriesManagementService.UpdateVoucherEntry("UPDATE", id, updateVoucherEntry);
            return RedirectToAction("VoucherEntryList", "VoucherEntries");
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveVoucherEntry(Guid id)
        {
            var hasVoucherEntry = await _voucherEntriesManagementService.GetVoucherEntryById("READBYID", id);
            if (hasVoucherEntry == null)
            {
                return NotFound("The voucher entry is not found!");
            }
            await _voucherEntriesManagementService.DeleteVoucherEntry("DELETE", id);
            return RedirectToAction("VoucherEntryList", "VoucherEntries");
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
