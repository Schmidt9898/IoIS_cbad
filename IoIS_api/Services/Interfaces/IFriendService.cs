using SocialApp.API.WebAPI.Models.Entities;
using SocialApp.API.WebAPI.ViewModels;
using SocialApp.Common;
using System.Linq.Expressions;

namespace SocialApp.API.WebAPI.Services.Interfaces
{
    public interface IFriendService
    {
        public Task<ICollection<UserVM>> GetAllFriendsWhereAsync(User user, Expression<Func<Friend, bool>> predicate);
        public Task<RequestState> AddFriendAsync(User user, string username);
        public Task<RequestState> UpdateFriendshipAsync(User user, string username, FriendState action);
    }
}
