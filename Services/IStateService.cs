using TomAndJerry.Model;

namespace TomAndJerry.Services;

public interface IStateService
{
    IEnumerable<Video> CurrentVideos { get; }
    IEnumerable<Video> FilteredVideos { get; }
    string CurrentSearchTerm { get; }
    bool IsLoading { get; }
    
    event Action? OnStateChanged;
    
    Task SetVideosAsync(IEnumerable<Video> videos);
    Task SetFilteredVideosAsync(IEnumerable<Video> videos);
    Task SetSearchTermAsync(string searchTerm);
    Task SetLoadingStateAsync(bool isLoading);
}
