using AutoMapper;
using SocialApp.API.WebAPI.Dtos;
using SocialApp.API.WebAPI.Models.Entities;
using SocialApp.API.WebAPI.ViewModels;

namespace SocialApp.API.WebAPI.Profiles
{
    // Ez az automapper miatt kell
    public class SocialAppProfile : Profile
    {
        public SocialAppProfile()
        {
            CreateMap<NewEventDto, Event>();
            CreateMap<UpdateEventDto, Event>();
            CreateMap<Event, EventVM>();
            CreateMap<Post, PostVM>()
                .ForMember(dst => dst.GroupName,
                           opt => opt.MapFrom(src => src.Group.Name));
            CreateMap<NewPostDto, Post>();

            CreateMap<User, UserVM>()
                .ForMember(dst => dst.FullName,
                           opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
        }
    }
}
