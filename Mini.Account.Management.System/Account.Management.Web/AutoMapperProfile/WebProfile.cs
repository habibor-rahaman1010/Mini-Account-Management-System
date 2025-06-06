﻿using Account.Management.Domain.Dtos;
using Account.Management.Domain.Entities;
using Account.Management.Infrastructure.Account.Management.Identity;
using Account.Management.Web.Areas.Admin.Models;
using AutoMapper;

namespace Account.Management.Web.AutoMapperProfile
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<UserUpdateModel, ApplicationUser>().ReverseMap();
            CreateMap<ChartOfAccountModel, ChartOfAccount>().ReverseMap();
            CreateMap<ChartOfAccountUpdate, ChartOfAccountUpdateDto>().ReverseMap();
            CreateMap<ChartOfAccountDto, ChartOfAccountUpdateDto>().ReverseMap();
            CreateMap<VoucherTypeModel, VoucherType>().ReverseMap();
            CreateMap<VoucherType, VoucherTypeDto>().ReverseMap();
            CreateMap<VoucherUpdateModel, VoucherType>().ReverseMap();
        }
    }
}
