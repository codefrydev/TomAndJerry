using TomAndJerry.Model;

namespace TomAndJerry.Services;

public class StateService : IStateService
{
    private IEnumerable<Video> _currentVideos = Enumerable.Empty<Video>();
    private IEnumerable<Video> _filteredVideos = Enumerable.Empty<Video>();
    private string _currentSearchTerm = string.Empty;
    private bool _isLoading = false;

    public IEnumerable<Video> CurrentVideos => _currentVideos;
    public IEnumerable<Video> FilteredVideos => _filteredVideos;
    public string CurrentSearchTerm => _currentSearchTerm;
    public bool IsLoading => _isLoading;

    public event Action? OnStateChanged;

    public async Task SetVideosAsync(IEnumerable<Video> videos)
    {
        _currentVideos = videos ?? Enumerable.Empty<Video>();
        await NotifyStateChangedAsync();
    }

    public async Task SetFilteredVideosAsync(IEnumerable<Video> videos)
    {
        _filteredVideos = videos ?? Enumerable.Empty<Video>();
        await NotifyStateChangedAsync();
    }

    public async Task SetSearchTermAsync(string searchTerm)
    {
        _currentSearchTerm = searchTerm ?? string.Empty;
        await NotifyStateChangedAsync();
    }

    public async Task SetLoadingStateAsync(bool isLoading)
    {
        _isLoading = isLoading;
        await NotifyStateChangedAsync();
    }

    private async Task NotifyStateChangedAsync()
    {
        await Task.Run(() => OnStateChanged?.Invoke());
    }
}
