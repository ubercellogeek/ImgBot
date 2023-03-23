namespace ImgBot.Api.Configuration
{
    public class SlackOptions
    {
        public const string Slack = "Slack";
        public const string PostMessageUri = "https://slack.com/api/chat.postMessage";
        public const string OAuth2AccessUri = "https://slack.com/api/oauth.v2.access";
        public const string OAuth2AuthorizeUri = "https://slack.com/oauth/v2/authorize";
        public string? OAuth2RedirectUri { get; set; }
        public string? SigningSecret { get; set; }
        public string? ClientSecret { get; set; }
        public string? ClientId { get; set; }
    }
}