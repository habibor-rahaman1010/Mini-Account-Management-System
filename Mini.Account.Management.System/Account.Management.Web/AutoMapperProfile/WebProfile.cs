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
        }
    }
}
