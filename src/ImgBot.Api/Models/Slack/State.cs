namespace ImgBot.Api.Models.Slack
{
    public class State
    {
        [JsonPropertyName("values")]
        public Values? Values { get; set; }
    }
}