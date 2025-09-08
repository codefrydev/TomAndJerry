using TomAndJerry.Model;

namespace TomAndJerry.Services;

public interface IStateService
{
    IEnumerable<Video> CurrentVideos { get; }
    IEnumerable<Video> FilteredVideos { get; }
    IEnumerable<Video> FeaturedVideos { get; }
    string CurrentSearchTerm { get; }
    bool IsLoading { get; }
    
    event Action? OnStateChanged;
    
    Task SetVideosAsync(IEnumerable<Video> videos);
    Task SetFilteredVideosAsync(IEnumerable<Video> videos);
    Task SetFeaturedVideosAsync(IEnumerable<Video> videos);
    Task RefreshFeaturedVideosAsync(IEnumerable<Video> allVideos, int count = 10);
    Task SetSearchTermAsync(string searchTerm);
    Task SetLoadingStateAsync(bool isLoading);
}
