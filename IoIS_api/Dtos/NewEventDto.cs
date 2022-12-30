using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.WebAPI.Dtos
{
    public class NewEventDto
    {
        [Required]
        public string Name { get; set; }
        [Required, DataType(DataType.DateTime)]
        public DateTime EventTime { get; set; }
        [MaxLength(50)]
        public string Location { get; set; }
        public string? Description { get; set; }
    }
}
