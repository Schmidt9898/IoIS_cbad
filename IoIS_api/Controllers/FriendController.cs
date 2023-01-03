using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialApp.API.WebAPI.Dtos;
using SocialApp.API.WebAPI.Models.Entities;
using SocialApp.API.WebAPI.Services.Interfaces;
using SocialApp.API.WebAPI.ViewModels;
using SocialApp.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialApp.API.WebAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<EventController> _logger;

        public FriendController(IFriendService friendService, ILogger<EventController> logger, UserManager<User> manager)
        {
            _friendService = friendService;
            _logger = logger;
            _userManager = manager;
        }

        // GET: api/<FriendController>
        [HttpGet("friends")]
        [Authorize]
        public async Task<IEnumerable<UserVM>> GetAllFriendsOfUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return await _friendService.GetAllFriendsWhereAsync(user, f => f.State == FriendState.Accepted);
        }

        [HttpGet("friend-requests")]
        [Authorize]
        public async Task<IEnumerable<UserVM>> GetAllFriendRequests()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return await _friendService.GetAllFriendsWhereAsync(user, f => f.State == FriendState.Pending);
        }

        // GET api/<FriendController>/5
        [HttpPost("friends/add/{username}")]
        [Authorize]
        public async Task<IActionResult> RequestFriendship(string username)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var result = await _friendService.AddFriendAsync(user, username);

            if (result == RequestState.Error)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { Status = "Bad request", Message = $"Parameter username must be different than requesting user ({user.UserName})" });

            if (result == RequestState.NotFound)
                return StatusCode(StatusCodes.Status404NotFound,
                    new { Status = "Not found", Message = "Requested user is not found." });

            if (result == RequestState.AlreadyExists)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { Status = "Bad request", Message = "Friendship already exists." });

            return StatusCode(StatusCodes.Status200OK,
                new { Status = "OK", Message = "Friend request sent." });
        }

        // Accept or decline or delete request
        [HttpPut("friends")]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] UpdateFriendDto dto)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var result = await _friendService.UpdateFriendshipAsync(user, dto.UserName, dto.Action);

            if (result == RequestState.Error)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { State = "Bad request", Message = "Cannot set friendship status to pending." });

            if (result == RequestState.NotFound)
                return StatusCode(StatusCodes.Status404NotFound,
                    new { State = "Not found", Message = "Friendship does not exist." });

            return StatusCode(StatusCodes.Status200OK,
                new { State = "OK", Message = "Friendship updated." });
        }
    }
}
