using SocialApp.API.WebAPI.Models.Entities;
using SocialApp.API.WebAPI.ViewModels;
using SocialApp.Common;

namespace SocialApp.API.WebAPI.Services.Interfaces
{
    public interface IFriendService
    {
        public Task<ICollection<UserVM>> GetAllFriendsAsync(User user);
        public Task<RequestState> AddFriendAsync(User user, string username);
    }
}
