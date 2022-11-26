using SocialApp.Common;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.WebAPI.Models.Entities
{
    public class Message
    {
        [Required]
        public virtual int Id { get; set; }
        [Required] 
        public virtual string FromUserId { get; set; }
        [Required]
        public virtual string ToUserId { get; set; }
        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
        [Required]
        public virtual string Content { get; set; }
        [Required]
        public DateTime SentAt { get; set; }
        [Required]
        public virtual MessageState State { get; set; }
    }
}
