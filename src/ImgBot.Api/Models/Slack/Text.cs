namespace ImgBot.Api.Models.Slack
{
    public class Text
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("text")]
        public string? TextValue { get; set; }

        [JsonPropertyName("emoji")]
        public bool Emoji { get; set; }
    }
}