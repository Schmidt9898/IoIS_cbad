using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.WebAPI.Models.Entities
{
    /// <summary>
    /// Implementation of the Group table via code-first approach
    /// </summary>
    public class Group
    {
        [Required]
        public virtual int Id { get; set; }
        [Required]
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string CoverImagePath { get; set; }
        [Required]
        public virtual DateTime CreatedAt { get; set; }
        [Required]
        public virtual User CreatedBy { get; set; }
        [Required]
        public virtual string CreatedById { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}
