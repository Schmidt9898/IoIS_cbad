using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.WebAPI.Models.Entities
{
    public class Post
    {
        [Required]
        public virtual int Id { get; set; }
        [Required]
        public virtual int GroupId { get; set; }
        public virtual Group Group { get; set; }
        [Required]
        public virtual string Content { get; set; }
        [Required]
        public virtual string CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
    }
}
