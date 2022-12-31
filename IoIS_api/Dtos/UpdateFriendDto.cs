using SocialApp.Common;

namespace SocialApp.API.WebAPI.Dtos
{
    public class UpdateFriendDto
    {
        public string UserName { get; set; }
        public FriendState Action { get; set; }
    }
}
