using TomAndJerry.Model;
using System.Collections.Concurrent;

namespace TomAndJerry.Services;

public class SearchService : ISearchService
{
    private readonly IVideoService _videoService;
    private readonly Timer _searchTimer;
    private string _currentSearchTerm = string.Empty;
    private readonly ConcurrentBag<Video> _searchResults = new();
    private readonly SemaphoreSlim _searchSemaphore = new(1, 1);

    public event Action<IEnumerable<Video>>? OnSearchResultsChanged;

    public SearchService(IVideoService videoService)
    {
        _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
        
        // Debounce search with 300ms delay
        _searchTimer = new Timer(PerformSearch, null, Timeout.Infinite, Timeout.Infinite);
    }

    public Task<IEnumerable<Video>> SearchAsync(string searchTerm)
    {
        _currentSearchTerm = searchTerm;
        
        // Reset timer to debounce rapid searches
        _searchTimer.Change(300, Timeout.Infinite);
        
        return Task.FromResult(_searchResults.AsEnumerable());
    }

    public async Task<IEnumerable<string>> GetSearchSuggestionsAsync(string partialTerm)
    {
        if (string.IsNullOrWhiteSpace(partialTerm) || partialTerm.Length < 2)
            return Enumerable.Empty<string>();

        var videos = await _videoService.GetAllVideosAsync();
        
        return videos
            .Select(v => v.Description)
            .Where(desc => desc.Contains(partialTerm, StringComparison.OrdinalIgnoreCase))
            .Take(5)
            .Distinct();
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
    }
}
