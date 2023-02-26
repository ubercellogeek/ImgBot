namespace ImgBot.Api.Models.Slack
{
    public class Team
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("domain")]
        public string? Domain { get; set; }
    }
}