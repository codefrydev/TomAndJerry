using TomAndJerry.Model;

namespace TomAndJerry.Services;

public interface IApplicationService
{
    Task InitializeApplicationAsync();
    Task<IEnumerable<Video>> GetAllVideosAsync();
    Task<Video?> GetVideoByIdAsync(string id);
    Task<IEnumerable<Video>> GetRandomVideosAsync(int count = 10);
    Task<IEnumerable<Video>> SearchVideosAsync(string searchTerm);
    Task<IEnumerable<string>> GetSearchSuggestionsAsync(string partialTerm);
    
    // State management
    IEnumerable<Video> CurrentVideos { get; }
    IEnumerable<Video> FilteredVideos { get; }
    string CurrentSearchTerm { get; }
    bool IsLoading { get; }
    bool IsInitialized { get; }
    
    // Events
    event Action? OnStateChanged;
    event Action<IEnumerable<Video>>? OnSearchResultsChanged;
    
    // State setters
    Task SetVideosAsync(IEnumerable<Video> videos);
    Task SetFilteredVideosAsync(IEnumerable<Video> videos);
    Task SetSearchTermAsync(string searchTerm);
    Task SetLoadingStateAsync(bool isLoading);
}
