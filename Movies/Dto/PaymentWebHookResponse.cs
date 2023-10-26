using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Movies.dto;

public class PaymentWebHookResponse
{
    [JsonProperty(PropertyName = "event")]
    public string? Events { get; set; }
    [JsonProperty(PropertyName = "data")]
    public DataSection? Data { get; set; }
}

public class DataSection
{
    [JsonProperty(PropertyName = "reference")]
    public string? Reference { get; set; }
    [JsonProperty(PropertyName = "amount")]
    public int Amount { get; set; }
    [JsonProperty(PropertyName = "status")]
    public string? Status { get; set; }
}

