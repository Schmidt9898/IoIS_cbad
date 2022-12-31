using System.Text.Json.Serialization;

namespace SocialApp.Common
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FriendState
    {
        Pending,
        Accepted,
        Declined,
    }
}