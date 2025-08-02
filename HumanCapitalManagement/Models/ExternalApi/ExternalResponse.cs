using System.Text.Json.Serialization;

namespace HumanCapitalManagement.Models.ExternalApi;

public class ExternalResponse
{
    [JsonPropertyName("num_working_days")]
    public int WorkingDays { get; set; }
}