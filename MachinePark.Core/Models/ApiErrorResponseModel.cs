using System.Text.Json;
using System.Text.Json.Serialization;

namespace MachinePark.Core.Models;

public class APIErrorResponseModel
{
    [JsonPropertyName("statusCode")]
    public int? StatusCode { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("errors")]
    public Dictionary<string, List<string>>? Errors { get; set; }
}