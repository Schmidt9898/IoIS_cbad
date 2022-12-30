using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialApp.API.WebAPI.Dtos;
using SocialApp.API.WebAPI.Models;
using SocialApp.API.WebAPI.Models.Entities;
using SocialApp.API.WebAPI.Services.Interfaces;
using SocialApp.API.WebAPI.ViewModels;
using SocialApp.Common;
using System.Linq;

namespace SocialApp.API.WebAPI.Services
{
    public class EventService : IEventService
    {
        private readonly SocialAppContext _context;
        private readonly IMapper _mapper;

        public EventService(SocialAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EventVM> CreateAsync(NewEventDto dto, User user)
        {
            try
            {
                var newEvent = _mapper.Map<Event>(dto);
                newEvent.CreatedBy = user;
                newEvent.CreatedAt = DateTime.Now;

                _context.Add(newEvent);
                await _context.SaveChangesAsync();

                var createdEvent = _mapper.Map<EventVM>(newEvent);
                return createdEvent;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<RequestState> DeleteAsync(int eventId, User user)
        {
            var toBeDeleted = await _context.Events
                .Where(e => e.Id == eventId)
                .FirstOrDefaultAsync();

            if (toBeDeleted == null)
            {
                return RequestState.NotFound;
            }
            if (toBeDeleted.CreatedById != user.Id)
                return RequestState.Unauthorized;

            _context.Remove(toBeDeleted);
            await _context.SaveChangesAsync();

            return RequestState.Successful;
        }

        public async Task<List<EventVM>> GetAllEventsAsync()
        {
            return await _context.Events
                .Select(e => _mapper.Map<EventVM>(e))
                .ToListAsync();
        }

        public async Task<List<EventVM>> GetAllEventsForUserAsync(User user)
        {
            return await _context.Events
                .Where(e => e.CreatedById == user.Id)
                .Select(e => _mapper.Map<EventVM>(e))
                .ToListAsync();
        }

        public async Task<EventVM> GetByIdAsync(int id)
        {
            return await _context.Events
                .Where(e => e.Id == id)
                .Select(e => _mapper.Map<EventVM>(e))
                .SingleOrDefaultAsync();
        }

        public async Task<RequestState> JoinAsync(int eventId, User user)
        {
            try
            {
                var e = await _context.Events
                    .Where(e => e.Id == eventId)
                    .FirstOrDefaultAsync();

                if (e == null)
                {
                    return RequestState.NotFound;
                }

                e.Users.Add(user);
                user.EventsParticipating.Add(e);

                await _context.SaveChangesAsync();

                return RequestState.Successful;
            }
            catch (DbUpdateException ex)
            {
                return RequestState.Error;
            }
        }

        public async Task<RequestState> LeaveAsync(int eventId, User user)
        {
            try
            {
                var e = await _context.Events
                    .Include(e => e.Users)
                    .Where(e => e.Id == eventId)
                    .FirstOrDefaultAsync();

                if (e == null || e.Users.Count == 0)
                    return RequestState.NotFound;

                e.Users.Remove(user);
                await _context.SaveChangesAsync();

                return RequestState.Successful;
            }
            catch (Exception ex)
            {
                return RequestState.Error;
            }
        }

        public async Task<RequestState> UpdateAsync(UpdateEventDto dto, User user)
        {
            var toBeUpdated = await _context.Events
                .Where(e => e.Id == dto.Id)
                .FirstOrDefaultAsync();

            if (toBeUpdated == null)
                return RequestState.NotFound;

            _mapper.Map(dto, toBeUpdated);
            toBeUpdated.ModifiedAt = DateTime.Now;
            _context.Entry(toBeUpdated).State = EntityState.Modified;

            if (toBeUpdated.CreatedById != user.Id)
                return RequestState.Unauthorized;

            await _context.SaveChangesAsync();

            return RequestState.Successful;
        }
    }
}
