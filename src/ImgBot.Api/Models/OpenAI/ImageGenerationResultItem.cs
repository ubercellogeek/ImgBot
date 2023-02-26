namespace ImgBot.Api.Models.OpenAI
{
    public class ImageGenerationResultItem
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("b64_json")]
        public string? Base64JsonString { get; set; }
    }
}