namespace ImgBot.Api.Models.ImgBot
{
    public class ImagePreview
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Url { get; set; }
    }
}