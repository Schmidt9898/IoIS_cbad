using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.WebAPI.Models.Entities
{
    public class UserGroup
    {
        [Required]
        public virtual int UserId { get; set; }
        [Required]
        public virtual int GroupId { get; set; }
        [Required]
        public virtual User User { get; set; }
        [Required]
        public virtual Group Group { get; set; }
        [Required]
        public virtual DateTime JoinedAt { get; set; }
    }
}
