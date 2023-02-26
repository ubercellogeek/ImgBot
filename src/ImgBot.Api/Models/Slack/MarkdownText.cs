namespace ImgBot.Api.Models.Slack
{
    public class MarkdownText
    {
        [JsonPropertyName("type")]
        public string Type => "mrkdwn";

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}