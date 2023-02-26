namespace ImgBot.Api.Models.Slack
{
    public class Image
    {
        [JsonPropertyName("type")]
        public string Type => "image";

        [JsonPropertyName("image_url")]
        public string? Url { get; set; }

        [JsonPropertyName("alt_text")]
        public string AltText { get; set; } = "";

    }
}