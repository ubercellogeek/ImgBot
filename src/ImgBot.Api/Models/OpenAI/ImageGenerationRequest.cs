namespace ImgBot.Api.Models.OpenAI
{
    public class ImageGenerationRequest
    {
        [JsonPropertyName("prompt")]
        public string? Prompt { get; set; }

        [JsonPropertyName("n")]
        public int Number { get; set; } = 1;

        [JsonPropertyName("size")]
        public string? Size { get; set; } = "512x512";

    }
}