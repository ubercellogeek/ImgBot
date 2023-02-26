namespace ImgBot.Api.Models.Slack
{
    public class PlainTextInput
    {
        [JsonPropertyName("type")]
        public string Type => "plain_text_input";

        [JsonPropertyName("action_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ActionId { get; set; }

        [JsonPropertyName("initial_value")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? InitialValue { get; set; }

        [JsonPropertyName("value")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Value { get; set; }

        [JsonPropertyName("multiline")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? IsMultiline { get; set; }

        [JsonPropertyName("placeholder")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public PlainText? Placeholder { get; set; }

    }
}