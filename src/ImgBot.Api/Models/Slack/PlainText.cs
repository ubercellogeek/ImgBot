namespace ImgBot.Api.Models.Slack
{
    public class PlainText
    {
        public PlainText() { }
        public PlainText(string text) => Text = text;

        [JsonPropertyName("type")]
        public string Type => "plain_text";

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("emoji")]
        public bool Emoji { get; set; }
    }
}