using AutoMapper.Configuration.Annotations;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.WebAPI.Models.Entities
{
    public class Event
    {
        public Event()
        {
            Users = new HashSet<User>();
        }

        [Required]
        public virtual int Id { get; set; }
        [Required, MaxLength(50)]
        public virtual string Name { get; set; }
        [Required]
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime ModifiedAt { get; set; }
        public virtual DateTime EventTime { get; set; }
        [MaxLength(50)]
        public virtual string Location { get; set; }
        public virtual string Description { get; set; }
        
        public virtual User CreatedBy { get; set; }
        [Required]
        public virtual string CreatedById { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
