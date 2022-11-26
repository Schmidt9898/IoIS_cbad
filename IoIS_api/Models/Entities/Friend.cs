using SocialApp.Common;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.WebAPI.Models.Entities
{
    public class Friend
    {
        [Required]
        public virtual string RequesterId { get; set; }
        [Required]
        public virtual string AddresseeId { get; set; }
        public virtual User Requester { get; set; }
        public virtual User Addressee { get; set; }
        [Required]
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime ModifiedAt { get; set; }
        [Required]
        public virtual FriendState State { get; set; }
    }
}
