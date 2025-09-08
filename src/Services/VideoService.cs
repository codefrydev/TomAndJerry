using TomAndJerry.Model;
using System.Collections.Concurrent;

namespace TomAndJerry.Services;

public class VideoService : IVideoService
{
    private readonly HttpClient _httpClient;
    private readonly ConcurrentBag<Video> _videos = new();
    private readonly SemaphoreSlim _initializationSemaphore = new(1, 1);
    private bool _isInitialized = false;

    public event Action? OnDataChanged;

    public bool IsInitialized => _isInitialized;

    public VideoService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized) return;

        await _initializationSemaphore.WaitAsync();
        try
        {
            if (_isInitialized) return;

            await FetchDataAsync();
            _isInitialized = true;
            OnDataChanged?.Invoke();
        }
        finally
        {
            _initializationSemaphore.Release();
        }
    }

    public async Task<IEnumerable<Video>> GetAllVideosAsync()
    {
        await EnsureInitializedAsync();
        return _videos.ToList();
    }

    public async Task<Video?> GetVideoByIdAsync(string id)
    {
        await EnsureInitializedAsync();
        return _videos.FirstOrDefault(x => x.Id == id);
    }

    public async Task<IEnumerable<Video>> GetRandomVideosAsync(int count = 10)
    {
        await EnsureInitializedAsync();
        var videos = _videos.ToList();
        return ShuffleArray(videos).Take(count);
    }

    public async Task<IEnumerable<Video>> SearchVideosAsync(string searchTerm)
    {
        await EnsureInitializedAsync();
        
        if (string.IsNullOrWhiteSpace(searchTerm))
            return _videos.ToList();

        return _videos.Where(video => 
            video.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            video.CommentName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private async Task EnsureInitializedAsync()
    {
        if (!_isInitialized)
        {
            await InitializeAsync();
        }
    }

    private async Task FetchDataAsync()
    {
        const string episodeNameUri = "https://raw.githubusercontent.com/TomJerry1940/Database/main/EpisodeNameList.txt";
        const string driveVideoUri = "https://raw.githubusercontent.com/TomJerry1940/Database/main/GoogleDriveVideoUri.txt";
        const string thumbnailPath = "https://raw.githubusercontent.com/TomJerry1940/Database/main/ThumbnailPath.txt";

        try
        {
            var videoListTask = _httpClient.GetStringAsync(driveVideoUri);
            var episodeNameTask = _httpClient.GetStringAsync(episodeNameUri);
            var thumbnailTask = _httpClient.GetStringAsync(thumbnailPath);

            await Task.WhenAll(videoListTask, episodeNameTask, thumbnailTask);

            var videoUriList = videoListTask.Result
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList();

            var episodeList = episodeNameTask.Result
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList();

            var thumbnailList = thumbnailTask.Result
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList();

            var maxCount = Math.Min(Math.Min(videoUriList.Count, episodeList.Count), thumbnailList.Count);

            for (var i = 0; i < maxCount; i++)
            {
                var video = new Video
                {
                    Id = $"{i + 1}",
                    Thumbnail = thumbnailList[i],
                    Description = episodeList[i],
                    VideoId = videoUriList[i],
                    CommentName = episodeList[i],
                    VideoUrl = videoUriList[i]
                };

                _videos.Add(video);
            }
        }
        catch (Exception ex)
        {
            // Log the exception in a real application
            Console.WriteLine($"Error fetching video data: {ex.Message}");
            throw;
        }
    }

    private static IEnumerable<T> ShuffleArray<T>(IEnumerable<T> array)
    {
        var random = new Random();
        var list = array.ToList();
        
        for (var i = 0; i < list.Count; i++)
        {
            var randIndex = random.Next(i, list.Count);
            (list[randIndex], list[i]) = (list[i], list[randIndex]);
        }

        return list;
    }
}
