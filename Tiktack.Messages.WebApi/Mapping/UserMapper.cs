using AutoMapper;
using Tiktack.Messaging.DataAccessLayer.Entities;
using Tiktack.Messaging.WebApi.DTOs;

namespace Tiktack.Messaging.WebApi.Mapping
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<ApplicationUser, UserDialogsDTO>()
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar.Url))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName));

            CreateMap<ApplicationUser, UserDetailsDTO>()
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar.Url))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName));
        }
    }
}
