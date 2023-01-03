using Microsoft.AspNetCore.Identity;
using SocialApp.API.WebAPI.Models.Entities;

namespace SocialApp.API.WebAPI.Models
{
    public static class SeedExtensions
    {
        public static IHost SeedUsers(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<User>>() ?? throw new ArgumentNullException();

                var testUser = new User
                {
                    FirstName = "Test",
                    LastName = "János",
                    UserName = "testj",
                    AboutMe = "Very descriptive about me section. I like developing full stack apps in my spare time.",
                    Email = "testj@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                userManager.CreateAsync(testUser, "Test123!").Wait();

                testUser = new User
                {
                    FirstName = "Test",
                    LastName = "József",
                    UserName = "testjo",
                    AboutMe = "Very descriptive about me section. I like long walks on the beach, reading, cooking, etc...",
                    Email = "testjo@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                userManager.CreateAsync(testUser, "Test123!").Wait();

                testUser = new User
                {
                    FirstName = "Test",
                    LastName = "Kata",
                    UserName = "testk",
                    Email = "testk@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                userManager.CreateAsync(testUser, "Test123!").Wait();
            }

            return host;
        }
    }
}
