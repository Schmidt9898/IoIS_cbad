using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialApp.API.WebAPI.Dtos;
using SocialApp.API.WebAPI.Models.Entities;
using SocialApp.API.WebAPI.Services.Interfaces;
using SocialApp.API.WebAPI.ViewModels;
using SocialApp.Common;
using ILogger = Serilog.ILogger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialApp.API.WebAPI.Controllers
{
    [ApiController]
    [Route("/events")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<EventController> _logger;

        public EventController(IEventService eventService, ILogger<EventController> logger, UserManager<User> manager)
        {
            _eventService = eventService;
            _logger = logger;
            _userManager = manager;
        }

        // GET: api/events
        [HttpGet]
        public async Task<IEnumerable<EventVM>> GetAllAsync()
        {
            _logger.LogInformation("Calling GetAllEventsAsync()...");
            return await _eventService.GetAllEventsAsync();
        }

        // GET api/<EventController>/5
        [HttpGet("user")]
        [Authorize]
        public async Task<IEnumerable<EventVM>> GetAllByUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return await _eventService.GetAllEventsForUserAsync(user);
        }

        [HttpDelete("leave/{id}")]
        public async Task<IActionResult> LeaveEvent(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var result = await _eventService.LeaveAsync(id, user);

            if (result == RequestState.NotFound)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status200OK,
                new { Status = "OK", Message = $"User {user.Id} successfully left event {id}." });
        }

        [HttpPost("join/{id}")]
        [Authorize]
        public async Task<IActionResult> JoinEvent(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var result = await _eventService.JoinAsync(id, user);

            if (result == RequestState.NotFound)
            {
                return NotFound();
            }
            if (result == RequestState.Error)
            {
                _logger.LogInformation($"User {user.Id} already joined for event {id}. Cannot fulfil request.");
                return StatusCode(StatusCodes.Status406NotAcceptable,
                    new { Satus = "Not Acceptable", Message = $"User {user.Id} already joined for event {id}." });
            }

            return StatusCode(StatusCodes.Status200OK,
                new { Status = "OK", Message = $"User {user.Id} successfully joined to event {id}."});
        }
        // POST api/<EventController>
        [HttpPost]
        public async Task<IActionResult> Post(NewEventDto eventDto)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var createdEvent = await _eventService.CreateAsync(eventDto, user);

            return CreatedAtAction(nameof(GetById), new { createdEvent.Id }, createdEvent);
        }

        // PUT api/<EventController>/5
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] UpdateEventDto dto)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var result = await _eventService.UpdateAsync(dto, user);

            if (result == RequestState.NotFound)
            {
                _logger.LogInformation($"Event with id {dto.Id} does not exist");
                return NotFound();
            }
            if (result == RequestState.Unauthorized)
            {
                _logger.LogInformation($"User {user.Id} is unauthorized to complete request.");
                return Unauthorized(user.Id);
            }

            return Ok();
        }

        // DELETE api/<EventController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await _eventService.DeleteAsync(id, user);

            if (result == RequestState.NotFound)
            {
                _logger.LogInformation($"Event with id {id} is not found");
                return NotFound();
            }
            if (result == RequestState.Unauthorized)
            {
                _logger.LogInformation($"User {user.Id} is unauthorized to complete request.");
                return Unauthorized(id);
            }

            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _eventService.GetByIdAsync(id);

            if (result is null)
                return NotFound();
            return Ok(result);
        }
    }
}
