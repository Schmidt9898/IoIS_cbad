using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.WebAPI.Dtos
{
    public class NewPostDto
    {
        public int? GroupId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
