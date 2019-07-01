using Auth0.AuthenticationApi.Models;
using AutoMapper;
using Tiktack.Messaging.DataAccessLayer.Entities;
using Tiktack.Messaging.WebApi.DTOs;

namespace Tiktack.Messaging.WebApi
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserInfo, UserInfoDBLayer>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.UserIdentifier, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Picture));
            CreateMap<UserInfoDBLayer, UserInfo>();
            CreateMap<UserInfoDBLayer, UserDTO>();
            CreateMap<ApplicationUser, UserDialogsDTO>()
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar.Url))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName));
        }
    }
}
