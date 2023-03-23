using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImgBot.Api.Models.Slack
{
    public class AuthToken
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        [JsonPropertyName("bot_user_id")]
        public string? BotUserId { get; set; }

        [JsonPropertyName("app_id")]
        public string? AppId { get; set; }

        [JsonPropertyName("team")]
        public Team Team { get; set; } = default!;

        [JsonPropertyName("enterprise")]
        public Enterprise? Enterprise { get; set; }

        [JsonPropertyName("authed_user")]
        public AuthedUser AuthedUser { get; set; } = default!;
    }
}