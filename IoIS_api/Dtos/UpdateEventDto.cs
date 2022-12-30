using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.WebAPI.Dtos
{
    public class UpdateEventDto
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EventTime { get; set; }
        [MaxLength(50)]
        public string Location { get; set; }
        public string? Description { get; set; }
    }
}
