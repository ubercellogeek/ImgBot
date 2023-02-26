using ImgBot.Api.Models.OpenAI;

namespace ImgBot.Api.Services
{
    public interface IOpenAIService
    {
        Task<ImageGenerationResult> GenerateImageAsync(ImageGenerationRequest request, CancellationToken cancellationToken);
    }
}