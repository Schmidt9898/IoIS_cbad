using SocialApp.API.WebAPI.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.WebAPI.ViewModels
{
    public class PostVM
    {
        public int Id { get; set; }
        public string? GroupName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
