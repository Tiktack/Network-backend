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
            CreateMap<ApplicationUser, UserDialogsDTO>()
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar.Url))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName));
        }
    }
}
