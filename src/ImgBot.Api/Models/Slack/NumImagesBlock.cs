namespace ImgBot.Api.Models.Slack
{
    public class NumImagesBlock
    {
        [JsonPropertyName("numImages")]
        public StaticSelectInput? NumImages { get; set; }
    }
}