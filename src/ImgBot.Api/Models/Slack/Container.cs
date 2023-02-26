namespace ImgBot.Api.Models.Slack
{
    public class Container
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("message_ts")]
        public string? MessageTimestamp { get; set; }

        [JsonPropertyName("channel_id")]
        public string? ChannelId { get; set; }

        [JsonPropertyName("is_ephemeral")]
        public bool IsEphemeral { get; set; }
    }
}