namespace ImgBot.Api.Models.Slack
{
    public class Message
    {
        [JsonPropertyName("blocks")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public object[]? Blocks { get; set; }

        [JsonPropertyName("username")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Username { get; set; }

        [JsonPropertyName("channel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Channel { get; set; }

        [JsonPropertyName("reply_broadcast")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? ReplyBroadcast { get; set; }

        [JsonPropertyName("replace_original")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? ReplaceOriginal { get; set; }

        [JsonPropertyName("response_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ResponseType { get; set; }

        [JsonPropertyName("thread_ts")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ThreadTs { get; set; }

        [JsonPropertyName("delete_original")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? DeleteMessage { get; set; }
    }
}