using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialApp.API.WebAPI.Models;
using SocialApp.API.WebAPI.Models.Entities;
using SocialApp.API.WebAPI.Services.Interfaces;
using SocialApp.API.WebAPI.ViewModels;
using SocialApp.Common;

namespace SocialApp.API.WebAPI.Services
{
    public class FriendService : IFriendService
    {
        private readonly SocialAppContext _context;
        private readonly IMapper _mapper;

        public FriendService(SocialAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RequestState> AddFriendAsync(User user, string username)
        {
            var friend = await _context.Users
                .Where(u => u.UserName == username)
                .FirstOrDefaultAsync();

            if (friend == null)
                return RequestState.NotFound;

            var friendship = await _context.Friends
                .Where(f => (f.RequesterId == user.Id && f.AddresseeId == friend.Id) || (f.AddresseeId == user.Id && f.RequesterId == friend.Id))
                .FirstOrDefaultAsync();

            if (friendship != null)
                return RequestState.AlreadyExists;

            friendship = new Friend
            {
                Requester = user,
                Addressee = friend,
                CreatedAt = DateTime.Now,
                State = FriendState.New
            };

            _context.Friends.Add(friendship);
            await _context.SaveChangesAsync();
            return RequestState.Successful;
        }

        public async Task<ICollection<UserVM>> GetAllFriendsAsync(User user)
        {
            // Get friends where the addressee or the requester is the user and the state is accepted
            // -> it does not matter who requested the friendship as long as it is accepted

            // btw ha erre van jobb ötleted, hogyan kéne azt szívesen fogadom...
            var friendsWhereUserIsRequester = await _context.Friends
                .Include(f => f.Addressee)
                .Where(f => f.RequesterId == user.Id && f.State == FriendState.Accepted)
                .Select(f => f.Addressee)
                .Select(f => _mapper.Map<UserVM>(f))
                .ToListAsync();

            var friendsWhereUserIsAddressee = await _context.Friends
                .Include(f => f.Requester)
                .Where(f => f.AddresseeId == user.Id && f.State == FriendState.Accepted)
                .Select(f => f.Requester)
                .Select(f => _mapper.Map<UserVM>(f))
                .ToListAsync();

            var concat = friendsWhereUserIsAddressee.Concat(friendsWhereUserIsRequester);
            return concat.ToList();
        }
    }
}
