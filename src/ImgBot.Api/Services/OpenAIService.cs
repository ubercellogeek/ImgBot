using ImgBot.Api.Configuration;
using ImgBot.Api.Models.OpenAI;
using Microsoft.Extensions.Options;

namespace ImgBot.Api.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly OpenAIOptions _openAIOptions;

        public OpenAIService(IHttpClientFactory clientFactory, IOptions<OpenAIOptions> options)
        {
            _clientFactory = clientFactory;
            _openAIOptions = options.Value;

            ArgumentNullException.ThrowIfNullOrEmpty(_openAIOptions.ApiKey);
        }

        public async Task<ImageGenerationResult> GenerateImageAsync(ImageGenerationRequest request, CancellationToken cancellationToken)
        {
            using var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _openAIOptions.ApiKey);

            var result = await client.PostAsJsonAsync(OpenAIOptions.ApiUri, request, new System.Text.Json.JsonSerializerOptions(), cancellationToken);
            var typedResult = await result.Content.ReadFromJsonAsync<ImageGenerationResult>();

            if(typedResult == default)
            {
                throw new Exception("Got null response back from OpenAI request");
            }
            typedResult.RawData = await result.Content.ReadAsStringAsync();
            typedResult.StatusCode = (int)result.StatusCode;

            return typedResult;
        }
    }
}