using TomAndJerry.Model;
using System.Collections.Concurrent;

namespace TomAndJerry.Services;

public class ApplicationService : IApplicationService
{
    private readonly IVideoService _videoService;
    private readonly ISearchService _searchService;
    private readonly IStateService _stateService;
    private readonly Timer _searchTimer;
    private string _currentSearchTerm = string.Empty;
    private readonly ConcurrentBag<Video> _searchResults = new();
    private readonly SemaphoreSlim _searchSemaphore = new(1, 1);

    public IEnumerable<Video> CurrentVideos => _stateService.CurrentVideos;
    public IEnumerable<Video> FilteredVideos => _stateService.FilteredVideos;
    public string CurrentSearchTerm => _stateService.CurrentSearchTerm;
    public bool IsLoading => _stateService.IsLoading;
    public bool IsInitialized => _videoService.IsInitialized;

    public event Action? OnStateChanged
    {
        add => _stateService.OnStateChanged += value;
        remove => _stateService.OnStateChanged -= value;
    }

    public event Action<IEnumerable<Video>>? OnSearchResultsChanged;

    public ApplicationService(IVideoService videoService, ISearchService searchService, IStateService stateService)
    {
        _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
        _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
        _stateService = stateService ?? throw new ArgumentNullException(nameof(stateService));
        
        // Debounce search with 300ms delay
        _searchTimer = new Timer(PerformSearch, null, Timeout.Infinite, Timeout.Infinite);
        
        // Subscribe to search service events
        _searchService.OnSearchResultsChanged += OnSearchResultsChanged;
    }

    public async Task InitializeApplicationAsync()
    {
        await _stateService.SetLoadingStateAsync(true);
        
        try
        {
            await _videoService.InitializeAsync();
            var videos = await _videoService.GetAllVideosAsync();
            await _stateService.SetVideosAsync(videos);
        }
        finally
        {
            await _stateService.SetLoadingStateAsync(false);
        }
    }

    public async Task<IEnumerable<Video>> GetAllVideosAsync()
    {
        return await _videoService.GetAllVideosAsync();
    }

    public async Task<Video?> GetVideoByIdAsync(string id)
    {
        return await _videoService.GetVideoByIdAsync(id);
    }

    public async Task<IEnumerable<Video>> GetRandomVideosAsync(int count = 10)
    {
        return await _videoService.GetRandomVideosAsync(count);
    }

    public Task<IEnumerable<Video>> SearchVideosAsync(string searchTerm)
    {
        _currentSearchTerm = searchTerm;
        
        // Reset timer to debounce rapid searches
        _searchTimer.Change(300, Timeout.Infinite);
        
        return Task.FromResult(_searchResults.AsEnumerable());
    }

    public async Task<IEnumerable<string>> GetSearchSuggestionsAsync(string partialTerm)
    {
        return await _searchService.GetSearchSuggestionsAsync(partialTerm);
    }

    public async Task SetVideosAsync(IEnumerable<Video> videos)
    {
        await _stateService.SetVideosAsync(videos);
    }

    public async Task SetFilteredVideosAsync(IEnumerable<Video> videos)
    {
        await _stateService.SetFilteredVideosAsync(videos);
    }

    public async Task SetSearchTermAsync(string searchTerm)
    {
        await _stateService.SetSearchTermAsync(searchTerm);
    }

    public async Task SetLoadingStateAsync(bool isLoading)
    {
        await _stateService.SetLoadingStateAsync(isLoading);
    }

    private async void PerformSearch(object? state)
    {
        await _searchSemaphore.WaitAsync();
        try
        {
            var results = await _videoService.SearchVideosAsync(_currentSearchTerm);
            
            _searchResults.Clear();
            foreach (var result in results)
            {
                _searchResults.Add(result);
            }
            
            OnSearchResultsChanged?.Invoke(_searchResults.ToList());
        }
        finally
        {
            _searchSemaphore.Release();
        }
    }

    public void Dispose()
    {
        _searchTimer?.Dispose();
        _searchSemaphore?.Dispose();
        _searchService.OnSearchResultsChanged -= OnSearchResultsChanged;
    }
}
