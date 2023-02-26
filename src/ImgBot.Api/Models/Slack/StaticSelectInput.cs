namespace ImgBot.Api.Models.Slack
{
    public class StaticSelectInput
    {
        [JsonPropertyName("type")]
        public string Type => "static_select";

        [JsonPropertyName("options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public object[]? Options { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("initial_option")]
        public object? InitialOption { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("placeholder")]
        public object? Placeholder { get; set; }

        [JsonPropertyName("action_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ActionId { get; set; }

        [JsonPropertyName("selected_option")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public SelectOption? SelectedOption { get; set; }
    }
}