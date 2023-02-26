namespace ImgBot.Api.Models.ImgBot
{
    public class ImagePreview
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string? Url { get; set; }
    }
}