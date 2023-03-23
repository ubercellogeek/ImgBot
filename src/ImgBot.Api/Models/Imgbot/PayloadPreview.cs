namespace ImgBot.Api.Models.ImgBot
{
    public class PayloadPreview
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Prompt { get; set; } = default!;
        public string ReplyToUri { get; set; } = default!;
        public List<ImagePreview> Images {get; set;} = new();
        public int RegenerateCount { get; set; }

    }
}