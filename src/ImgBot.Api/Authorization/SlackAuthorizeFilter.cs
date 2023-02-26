using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using ImgBot.Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace ImgBot.Api.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class SlackAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var headers = context.HttpContext.Request.Headers;
            var encoding = new UTF8Encoding();
            var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<SlackOptions>>();

            // Read from the body stream
            context.HttpContext.Request.EnableBuffering(); // Enable reuse of the body stream
            var requestBody = await (new StreamReader(context.HttpContext.Request.Body)).ReadToEndAsync();
            context.HttpContext.Request.Body.Position = 0; // Reset the body stream

            // Compute the HMAC256 signature as documented here: https://api.slack.com/authentication/verifying-requests-from-slack
            using var hmac = new HMACSHA256(encoding.GetBytes(options.Value.SigningSecret!));
            var hash = hmac.ComputeHash(encoding.GetBytes($"v0:{headers["X-Slack-Request-Timestamp"]}:{requestBody}"));
            var hashString = $"v0={BitConverter.ToString(hash).Replace("-", "").ToLower(CultureInfo.InvariantCulture)}";

            if(!hashString.Equals(headers["X-Slack-Signature"]))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}