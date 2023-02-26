namespace ImgBot.Api.Models.Slack
{
    public class Values
    {
        [JsonPropertyName("blk_prompt")]
        public PromptBlock? Prompt { get; set; }

         [JsonPropertyName("blk_numImages")]
        public NumImagesBlock? NumImages { get; set; }
    }
}