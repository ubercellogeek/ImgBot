namespace ImgBot.Api.Models.Slack
{
    public class MarkdownText
    {
        public MarkdownText()
        {

        }

        public MarkdownText(string text)
        {
            Text = text;
        }
        [JsonPropertyName("type")]
        public string Type => "mrkdwn";

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}