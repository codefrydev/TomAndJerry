using TomAndJerry.Model;

namespace TomAndJerry.Services
{
    public class StickerService : IStickerService
    {
        private List<Sticker> _stickers = new();
        private readonly Random _random = new();

        public async Task<List<Sticker>> GetAllStickersAsync()
        {
            if (!_stickers.Any())
            {
                await LoadStickersAsync();
            }
            return _stickers;
        }

        public async Task<Sticker> GetRandomStickerAsync()
        {
            var stickers = await GetAllStickersAsync();
            if (!stickers.Any())
                return new Sticker { FileName = "Tom.png", ImagePath = "Tom.png", DisplayName = "Tom" };

            return stickers[_random.Next(stickers.Count)];
        }

        public async Task<List<Sticker>> GetRandomStickersAsync(int count)
        {
            var stickers = await GetAllStickersAsync();
            if (!stickers.Any())
                return new List<Sticker> { new Sticker { FileName = "Tom.png", ImagePath = "Tom.png", DisplayName = "Tom" } };

            return stickers.OrderBy(x => _random.Next()).Take(count).ToList();
        }

        public async Task<Sticker> GetStickerByIdAsync(int id)
        {
            var stickers = await GetAllStickersAsync();
            return stickers.FirstOrDefault(s => s.Id == id) ?? 
                   new Sticker { FileName = "Tom.png", ImagePath = "Tom.png", DisplayName = "Tom" };
        }

        private async Task LoadStickersAsync()
        {
            _stickers = new List<Sticker>
            {
                new Sticker { Id = 1, FileName = "sticker.webp", ImagePath = "sticker/sticker.webp", DisplayName = "Tom & Jerry Classic", Category = "classic" },
                new Sticker { Id = 2, FileName = "sticker_(2).webp", ImagePath = "sticker/sticker_(2).webp", DisplayName = "Tom & Jerry Adventure", Category = "adventure" },
                new Sticker { Id = 3, FileName = "sticker_(3).webp", ImagePath = "sticker/sticker_(3).webp", DisplayName = "Tom & Jerry Chase", Category = "chase" },
                new Sticker { Id = 4, FileName = "sticker_(4).webp", ImagePath = "sticker/sticker_(4).webp", DisplayName = "Tom & Jerry Fun", Category = "fun" },
                new Sticker { Id = 5, FileName = "sticker_(5).webp", ImagePath = "sticker/sticker_(5).webp", DisplayName = "Tom & Jerry Comedy", Category = "comedy" },
                new Sticker { Id = 6, FileName = "sticker_(6).webp", ImagePath = "sticker/sticker_(6).webp", DisplayName = "Tom & Jerry Classic 2", Category = "classic" },
                new Sticker { Id = 7, FileName = "sticker_(7).webp", ImagePath = "sticker/sticker_(7).webp", DisplayName = "Tom & Jerry Action", Category = "action" },
                new Sticker { Id = 8, FileName = "sticker_(8).webp", ImagePath = "sticker/sticker_(8).webp", DisplayName = "Tom & Jerry Mischief", Category = "mischief" },
                new Sticker { Id = 9, FileName = "sticker_(9).webp", ImagePath = "sticker/sticker_(9).webp", DisplayName = "Tom & Jerry Playful", Category = "playful" },
                new Sticker { Id = 10, FileName = "sticker_(10).webp", ImagePath = "sticker/sticker_(10).webp", DisplayName = "Tom & Jerry Vintage", Category = "vintage" },
                new Sticker { Id = 11, FileName = "sticker_(11).webp", ImagePath = "sticker/sticker_(11).webp", DisplayName = "Tom & Jerry Cute", Category = "cute" },
                new Sticker { Id = 12, FileName = "sticker_(12).webp", ImagePath = "sticker/sticker_(12).webp", DisplayName = "Tom & Jerry Dynamic", Category = "dynamic" },
                new Sticker { Id = 13, FileName = "sticker_(13).webp", ImagePath = "sticker/sticker_(13).webp", DisplayName = "Tom & Jerry Energetic", Category = "energetic" },
                new Sticker { Id = 14, FileName = "sticker_(14).webp", ImagePath = "sticker/sticker_(14).webp", DisplayName = "Tom & Jerry Cheerful", Category = "cheerful" },
                new Sticker { Id = 15, FileName = "sticker_(15).webp", ImagePath = "sticker/sticker_(15).webp", DisplayName = "Tom & Jerry Lively", Category = "lively" },
                new Sticker { Id = 16, FileName = "sticker_(16).webp", ImagePath = "sticker/sticker_(16).webp", DisplayName = "Tom & Jerry Spirited", Category = "spirited" },
                new Sticker { Id = 17, FileName = "sticker_(17).webp", ImagePath = "sticker/sticker_(17).webp", DisplayName = "Tom & Jerry Joyful", Category = "joyful" },
                new Sticker { Id = 18, FileName = "sticker_(18).webp", ImagePath = "sticker/sticker_(18).webp", DisplayName = "Tom & Jerry Exciting", Category = "exciting" },
                new Sticker { Id = 19, FileName = "sticker_(19).webp", ImagePath = "sticker/sticker_(19).webp", DisplayName = "Tom & Jerry Animated", Category = "animated" },
                new Sticker { Id = 20, FileName = "sticker_(20).webp", ImagePath = "sticker/sticker_(20).webp", DisplayName = "Tom & Jerry Colorful", Category = "colorful" },
                new Sticker { Id = 21, FileName = "sticker_(21).webp", ImagePath = "sticker/sticker_(21).webp", DisplayName = "Tom & Jerry Bright", Category = "bright" },
                new Sticker { Id = 22, FileName = "sticker_(22).webp", ImagePath = "sticker/sticker_(22).webp", DisplayName = "Tom & Jerry Vibrant", Category = "vibrant" },
                new Sticker { Id = 23, FileName = "sticker_(23).webp", ImagePath = "sticker/sticker_(23).webp", DisplayName = "Tom & Jerry Playful 2", Category = "playful" },
                new Sticker { Id = 24, FileName = "sticker_(24).webp", ImagePath = "sticker/sticker_(24).webp", DisplayName = "Tom & Jerry Special", Category = "special" }
            };

            await Task.CompletedTask;
        }
    }
}
