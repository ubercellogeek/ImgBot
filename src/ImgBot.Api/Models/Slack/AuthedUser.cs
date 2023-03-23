using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImgBot.Api.Models.Slack
{
    public class AuthedUser
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        [JsonPropertyName("scope")]
        public string Scope { get; set; } = default!;

        [JsonPropertyName("access_token")] 
        public string AccessToken { get; set; } = default!;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = default!;
    }
}