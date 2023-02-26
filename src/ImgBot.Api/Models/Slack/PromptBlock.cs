namespace ImgBot.Api.Models.Slack
{
    public class PromptBlock
    {
        [JsonPropertyName("prompt")]
        public PlainTextInput? Prompt { get; set; }
    }
}