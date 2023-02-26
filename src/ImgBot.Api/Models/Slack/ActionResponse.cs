namespace ImgBot.Api.Models.Slack
{
    public class ActionResponse
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("user")]
        public User? User { get; set; }

        [JsonPropertyName("api_app_id")]
        public string? ApiAppId { get; set; }

        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [JsonPropertyName("container")]
        public Container? Container { get; set; }

        [JsonPropertyName("trigger_id")]
        public string? TriggerId { get; set; }

        [JsonPropertyName("team")]
        public Team? Team { get; set; }

        [JsonPropertyName("enterprise")]
        public object? Enterprise { get; set; }

        [JsonPropertyName("is_enterprise_install")]
        public bool IsEnterpriseInstall { get; set; }

        [JsonPropertyName("channel")]
        public Channel? Channel { get; set; }

        [JsonPropertyName("state")]
        public State? State { get; set; }

        [JsonPropertyName("response_url")]
        public string? ResponseUrl { get; set; }

        [JsonPropertyName("actions")]
        public List<Action>? Actions { get; set; }
    }
}