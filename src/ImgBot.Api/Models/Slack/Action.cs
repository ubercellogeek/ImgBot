namespace ImgBot.Api.Models.Slack
{
    public class Action
    {
        [JsonPropertyName("action_id")]
        public string? ActionId { get; set; }

        [JsonPropertyName("block_id")]
        public string? BlockId { get; set; }

        [JsonPropertyName("text")]
        public Text? Text { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("style")]
        public string? Style { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("action_ts")]
        public string? ActionTimestamp { get; set; }
    }
}