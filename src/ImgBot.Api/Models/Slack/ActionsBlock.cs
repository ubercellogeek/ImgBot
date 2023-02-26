namespace ImgBot.Api.Models.Slack
{
    public class ActionsBlock
    {
        [JsonPropertyName("type")]
        public string Type => "actions";

        [JsonPropertyName("elements")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public object[]? Elements { get; set; }

        [JsonPropertyName("block_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? BlockId { get; set; }
    }
}