using System.Text.Json.Serialization;

namespace SaltOverFlowApi.Models;

public class CommentRequest
{
    [JsonPropertyName("text")] public string Text { get; set; }
    [JsonPropertyName("userId")] public int? UserId { get; set; }
    [JsonPropertyName("postId")] public int? PostId { get; set; }
}