using TomAndJerry.Model;

namespace TomAndJerry.Services
{
    public interface IStickerService
    {
        Task<List<Sticker>> GetAllStickersAsync();
        Task<Sticker> GetRandomStickerAsync();
        Task<List<Sticker>> GetRandomStickersAsync(int count);
        Task<Sticker> GetStickerByIdAsync(int id);
    }
}
