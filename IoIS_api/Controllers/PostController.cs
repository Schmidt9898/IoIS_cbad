using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialApp.API.WebAPI.Dtos;
using SocialApp.API.WebAPI.Models.Entities;
using SocialApp.API.WebAPI.Services;
using SocialApp.API.WebAPI.Services.Interfaces;
using SocialApp.API.WebAPI.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialApp.API.WebAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<EventController> _logger;

        public PostController(IPostService postService, ILogger<EventController> logger, UserManager<User> manager)
        {
            _postService = postService;
            _logger = logger;
            _userManager = manager;
        }

        // GET: api/<PostController>
        [HttpGet("{username}/posts")]
        public async Task<IEnumerable<PostVM>> GetAllPostsOfUser(string username)
        {
            return await _postService.GetPostsOfUserAsync(username);
        }

        [HttpPost("posts")]
        public async Task<IActionResult> Post([FromBody] NewPostDto dto)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var result = await _postService.CreateAsync(dto, user);

            if (result == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new { State = "Not found", Message = $"Group {dto.GroupId} does not exist." });

            return CreatedAtAction(nameof(GetById), new { result.Id }, result);
        }

        [HttpGet("posts/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _postService.GetByIdAsync(id);

            if (result is null)
                return NotFound();
            return Ok(result);
        }
    }
}
