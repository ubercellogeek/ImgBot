namespace ImgBot.Api.Models.Slack
{
    public class Section
    {
        [JsonPropertyName("type")]
        public string Type => "section";

        [JsonPropertyName("text")]
        public object? Text { get; set; }
    }
}