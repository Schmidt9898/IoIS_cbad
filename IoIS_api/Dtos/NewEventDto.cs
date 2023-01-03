using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.WebAPI.Dtos
{
    public class NewEventDto
    {
        [Required]
        public string Name { get; set; }
        [Required, DataType(DataType.DateTime)]
        public DateTime EventTime { get; set; }
        [Required, MaxLength(50)]
        public string Location { get; set; }
        [Required]
        public string? Description { get; set; }
    }
}
