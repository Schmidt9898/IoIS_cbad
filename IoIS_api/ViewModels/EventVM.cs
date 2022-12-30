using SocialApp.API.WebAPI.Models.Entities;

namespace SocialApp.API.WebAPI.ViewModels
{
    public class EventVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EventTime { get; set; }

        public string Location { get; set; }
        public string Description { get; set; }
        public string CreatedById { get; set; }
    }
}
