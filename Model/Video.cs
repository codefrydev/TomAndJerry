namespace TomAndJerry.Model
{
    public class Video
    {
        public int Id { get; set; }
        public string Thumbnail { get; set; } = string.Empty;
        public string CommentName { get; set; } = string.Empty;

        public string VideoId { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string VideoUrl { get; set; } = string.Empty;
    }
}
