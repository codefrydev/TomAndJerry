namespace TomAndJerry.Model
{
    public class Sticker
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string Category { get; set; } = "general";
        public bool IsAnimated { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
