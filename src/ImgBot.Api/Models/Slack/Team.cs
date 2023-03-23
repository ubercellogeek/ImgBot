namespace ImgBot.Api.Models.Slack
{
    public class Team
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        [JsonPropertyName("domain")]
        public string? Domain { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;
    }
}