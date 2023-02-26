namespace ImgBot.Api.Models.OpenAI
{
    public class ImageGenerationResult
    {
        [JsonPropertyName("created")]
        public long Created { get; set; }
        
        [JsonPropertyName("data")]
        public List<ImageGenerationResultItem> Items { get; set; } = new();

        [JsonIgnore]
        public string? RawData { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }
    }
}