namespace ImgBot.Api.Configuration
{
    public class OpenAIOptions
    {
        public static string ApiUri = "https://api.openai.com/v1/images/generations";
        public const string OpenAI = "OpenAI";
        public string? ApiKey { get; set; }
    }
}