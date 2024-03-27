using Newtonsoft.Json;

namespace Core.Helpers;

public class RecaptchaResponse
{
    [JsonProperty("success")]
    public bool success { get; set; }
    [JsonProperty("challenge_ts")]
    public DateTime challenge_ts { get; set; }
    [JsonProperty("hostname")]
    public string hostname { get; set; }
}