using System.Text.Json;
using System.Text.Json.Serialization;

namespace MachinePark.Core.Models;

public class APISuccessResponseModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

}