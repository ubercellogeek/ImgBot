using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImgBot.Api.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using ImgBot.Api.Models.Slack;
using ImgBot.Api.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace ImgBot.Api.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IHttpClientFactory _httpFactory;
        private readonly SlackOptions _options;
        private readonly IDistributedCache _cache;
        private readonly ISlackService _slackService;


        public AuthenticationController(ILogger<AuthenticationController> logger, IHttpClientFactory httpFactory, IOptions<SlackOptions> options, IDistributedCache cache, ISlackService slackService)
        {
            _logger = logger;
            _httpFactory = httpFactory;
            _options = options.Value;
            _cache = cache;
            _slackService = slackService;
        }

        [HttpGet("/auth/v2")]
        public async Task<IActionResult> Authv2(string code, string state)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("code", code);


            using var client = _httpFactory.CreateClient();

            var authenticationString = $"{_options.ClientId}:{_options.ClientSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, SlackOptions.OAuth2AccessUri);
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
                requestMessage.Content = new FormUrlEncodedContent(dict);

            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
            var token = await response.Content.ReadFromJsonAsync<AuthToken>();

            var context = new TokenContext()
            {
                UserId = token.AuthedUser.Id,
                TeamId = token.Team.Id,
                EnterpriseId = token.Enterprise?.Id,
                UserToken = token.AuthedUser.AccessToken
            };

            await _slackService.SetTokenContextAsync(context);
            var result = await _slackService.GetTokenContextAsync(context.UserId, context.TeamId, context.EnterpriseId);

            string? responseUri = default;

            if(!string.IsNullOrWhiteSpace(state))
            {
                responseUri = await _cache.GetStringAsync(state);
            }
        

            if(!string.IsNullOrWhiteSpace(state) && !string.IsNullOrWhiteSpace(responseUri))
            {
                // Only get here if this was initiated in-app.
                await _cache.RemoveAsync(state);

                var message = new Message();
                message.Blocks = new object[] {
                new Section() {
                    Text = new MarkdownText("You're all set! You can now generate images by simply issuing a slash command. Example: `/imagebot A rocket launching at dawn, backlit.`")
                },
                new ActionsBlock() {
                    Elements = new object[] {
                        new Button() { Style = "primary", Text = new PlainText("Got it"),ActionId = "ok_auth_complete", Value = "ok_auth_complete" },
                    }
                }};

                await _slackService.SendPayload(responseUri, message, default);
            }

            return Redirect("/complete");
        }
    }
}