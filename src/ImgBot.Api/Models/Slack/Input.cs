namespace ImgBot.Api.Models.Slack
{
    public class Input
    {
        [JsonPropertyName("type")]
        public string Type => "input";

        [JsonPropertyName("element")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public object? Element { get; set; }

        [JsonPropertyName("label")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public object? Label { get; set; }

        [JsonPropertyName("block_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? BlockId { get; set; }
    }
}