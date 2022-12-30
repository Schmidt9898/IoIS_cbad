using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialApp.API.WebAPI.Models.Entities;
using SocialApp.API.WebAPI.Services.Interfaces;
using SocialApp.API.WebAPI.ViewModels;
using SocialApp.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialApp.API.WebAPI.Controllers
{
    [Route("api/friends")]
    [ApiController]
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
        [HttpGet]
        public async Task<IEnumerable<UserVM>> GetAllFriendsOfUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return await _friendService.GetAllFriendsAsync(user);
        }

        // GET api/<FriendController>/5
        [HttpPost("{username}")]
        public async Task<IActionResult> RequestFriendship(string username)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var result = await _friendService.AddFriendAsync(user, username);

            if (result == RequestState.NotFound)
                return StatusCode(StatusCodes.Status404NotFound,
                    new { Status = "Not found", Message = "Requested user is not found." });

            if (result == RequestState.AlreadyExists)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { Status = "Bad request", Message = "Friendship already exists." });

            return StatusCode(StatusCodes.Status200OK,
                new { Status = "OK", Message = "Friend request sent." });
        }

        // POST api/<FriendController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FriendController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FriendController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
