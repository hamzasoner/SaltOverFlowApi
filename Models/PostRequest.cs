using System.Text.Json.Serialization;

namespace SaltOverFlowApi.Models;

public class PostRequest
{
    [JsonPropertyName("userId")] public int UserId { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("content")] public string Content { get; set; }
    [JsonPropertyName("tags")] public string Tags { get; set; }

    [JsonPropertyName("image")] public byte[]? Image { get; set; }

    [JsonPropertyName("vote")] public int Vote { get; set; }
}