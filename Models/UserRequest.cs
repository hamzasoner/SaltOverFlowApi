using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SaltOverFlowApi.Models;

public class UserRequest
{
    [Required] [JsonPropertyName("name")] public string? Name { get; set; }

    [EmailAddress]
    [JsonPropertyName("email")]
    public string? Email { get; set; }
}