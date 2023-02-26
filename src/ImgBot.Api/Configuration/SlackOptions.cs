namespace ImgBot.Api.Configuration
{
    public class SlackOptions
    {
        public const string Slack = "Slack";
        public const string PostMessageUri = "https://slack.com/api/chat.postMessage";
        public string? UserOAuthToken { get; set; }
        public string? SigningSecret { get; set; }
    }
}