using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.WebAPI.Models.Entities
{
    /// <summary>
    /// Identity package manages all the basic stuff for authentication.
    /// We just need to define the app-specific properties of a user.
    /// </summary>
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string ProfilePicturePath { get; set; }
        [Required]
        public string AboutMe { get; set; }
        public virtual ICollection<Event> CreatedEvents { get; set; }
        public virtual ICollection<Post> CreatedPosts { get; set; }
        public virtual ICollection<Event> EventsParticipating { get; set; }
        public virtual ICollection<Group> CreatedGroups { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<Friend> Requesters { get; set; }
        public virtual ICollection<Friend> Addressees { get; set; }
        public virtual ICollection<Message> MessageFrom { get; set; }
        public virtual ICollection<Message> MessageTo { get; set; }
    }
}
