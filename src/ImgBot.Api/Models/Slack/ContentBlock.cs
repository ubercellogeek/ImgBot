namespace ImgBot.Api.Models.Slack
{
    public class ContextBlock
    {
        [JsonPropertyName("type")]
        public string Type => "context";

        [JsonPropertyName("elements")]
        public object[]? Elements { get; set; }
    }
}