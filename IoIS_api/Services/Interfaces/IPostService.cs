using SocialApp.API.WebAPI.Dtos;
using SocialApp.API.WebAPI.Models.Entities;
using SocialApp.API.WebAPI.ViewModels;

namespace SocialApp.API.WebAPI.Services.Interfaces
{
    public interface IPostService
    {
        public Task<ICollection<PostVM>> GetPostsOfUserAsync(string username);
        public Task<PostVM> GetByIdAsync(int id);
        public Task<PostVM> CreateAsync(NewPostDto dto, User user);
    }
}
