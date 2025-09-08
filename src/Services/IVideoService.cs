using TomAndJerry.Model;

namespace TomAndJerry.Services;

public interface IVideoService
{
    Task<IEnumerable<Video>> GetAllVideosAsync();
    Task<Video?> GetVideoByIdAsync(string id);
    Task<IEnumerable<Video>> GetRandomVideosAsync(int count = 10);
    Task<IEnumerable<Video>> SearchVideosAsync(string searchTerm);
    Task InitializeAsync();
    bool IsInitialized { get; }
    event Action? OnDataChanged;
}
