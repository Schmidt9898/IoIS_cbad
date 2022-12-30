using SocialApp.API.WebAPI.Dtos;
using SocialApp.API.WebAPI.Models.Entities;
using SocialApp.API.WebAPI.ViewModels;
using SocialApp.Common;

namespace SocialApp.API.WebAPI.Services.Interfaces
{
    public interface IEventService
    {
        public Task<EventVM> GetByIdAsync(int id);
        public Task<List<EventVM>> GetAllEventsAsync();
        public Task<List<EventVM>> GetAllEventsForUserAsync(User user);
        public Task<EventVM> CreateAsync(NewEventDto dto, User user);
        public Task<RequestState> UpdateAsync(UpdateEventDto dto, User user);
        public Task<RequestState> DeleteAsync(int eventId, User user);
        public Task<RequestState> JoinAsync(int eventId, User user);
        public Task<RequestState> LeaveAsync(int eventId, User user);
    }
}
