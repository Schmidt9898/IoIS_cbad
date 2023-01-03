using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialApp.API.WebAPI.Models;
using SocialApp.API.WebAPI.Models.Entities;
using SocialApp.API.WebAPI.Services.Interfaces;
using SocialApp.API.WebAPI.ViewModels;
using SocialApp.Common;
using System.Linq.Expressions;

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
            if (username == user.UserName)
                return RequestState.Error;

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
                State = FriendState.Pending
            };

            _context.Friends.Add(friendship);
            await _context.SaveChangesAsync();
            return RequestState.Successful;
        }

        public async Task<ICollection<UserVM>> GetAllFriendsWhereAsync(User user, Expression<Func<Friend, bool>> predicate)
        {
            // Get friends where the addressee or the requester is the user and the state is accepted
            // -> it does not matter who requested the friendship as long as it is accepted

            // btw ha erre van jobb ötleted, hogyan kéne azt szívesen fogadom...
            var friendsWhereUserIsRequester = await _context.Friends
                .Include(f => f.Addressee)
                .Where(predicate)
                .Where(f => f.RequesterId == user.Id)
                .Select(f => f.Addressee)
                .Select(f => _mapper.Map<UserVM>(f))
                .ToListAsync();

            var friendsWhereUserIsAddressee = await _context.Friends
                .Include(f => f.Requester)
                .Where(predicate)
                .Where(f => f.AddresseeId == user.Id)
                .Select(f => f.Requester)
                .Select(f => _mapper.Map<UserVM>(f))
                .ToListAsync();

            var concat = friendsWhereUserIsAddressee.Concat(friendsWhereUserIsRequester);
            return concat.ToList();
        }

        public async Task<RequestState> UpdateFriendshipAsync(User user, string username, FriendState action)
        {
            // Cannot set state to pending
            if (action == FriendState.Pending)
                return RequestState.Error;

            // Get only pending requests
            var friendship = await _context.Friends
                .Where(f => f.Requester.UserName == username && f.Addressee == user && f.State == FriendState.Pending)
                .FirstOrDefaultAsync();

            if (friendship is null)
                return RequestState.NotFound;

            friendship.State = action;
            friendship.ModifiedAt = DateTime.Now;

            _context.Entry(friendship).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return RequestState.Successful;
        }
    }
}
