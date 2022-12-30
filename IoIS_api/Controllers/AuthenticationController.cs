using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialApp.API.WebAPI.Dtos.Authentication;
using SocialApp.API.WebAPI.Models.Entities;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SocialApp.API.WebAPI.ViewModels;
using AutoMapper;

namespace SocialApp.API.WebAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<User> userManager, ILogger<AuthenticationController> logger, IConfiguration configuration)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            // Find user by email
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            // If user exists and password is correct, authorize, give claims, token
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

                return Ok(new LoginVM
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expires = token.ValidTo
                });
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByNameAsync(registerDto.UserName);

            if (existingUser != null)
            {
                _logger.LogError($"User creation failed. There is an existing user with username {registerDto.UserName}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Status = "Error", Message = "User creation failed. There is a user registered with this email or username." });
            }

            var newUser = new User
            {
                UserName = registerDto.UserName,
                FirstName= registerDto.FirstName,
                LastName= registerDto.LastName,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var creationResult = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!creationResult.Succeeded)
            {
                _logger.LogError($"User creation failed. Message: {creationResult.Errors.FirstOrDefault()}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Status = "Error", Message = "Error during user creation.", InnerException = $"{creationResult.Errors.FirstOrDefault()}" });
            }

            _logger.LogInformation("User successfully created.");
            return Ok(new { Status = "Success", Message = "User successfully created." });
        }
    }
}
