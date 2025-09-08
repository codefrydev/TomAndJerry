using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TomAndJerry.Model;

namespace TomAndJerry.Pages;

public partial class PlayMedia : IDisposable
{
    bool show = false;
    string activeTab = "giscus";
    [Parameter] public string VideId { get; set; } = string.Empty;
    
    private Video? currentVideo;
    private List<Video> randomVideos = new();
    private int currentVideoCount = 8;
    private int videosPerLoad = 8;
    private bool isLoadingMore = false;
    private bool hasMoreVideos = true;

    private void GoTOPage(Video video)
    {
        nav.NavigateTo($"playmedia/{video.Id}");
    }

    async Task LoadGiscud(string id)
    {
        await JS.InvokeVoidAsync("loadDisqus", nav.Uri, id);
    }

    void ShowComment()
    {
        show = false;
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        StateService.OnStateChanged += StateHasChanged;
        await LoadVideoData();
    }
    
    protected override async Task OnParametersSetAsync()
    {
        await LoadVideoData();
    }
    
    private async Task LoadVideoData()
    {
        await StateService.SetLoadingStateAsync(true);
        
        try
        {
            await VideoService.InitializeAsync();
            currentVideo = await VideoService.GetVideoByIdAsync(VideId);
            
            randomVideos.Clear();
            currentVideoCount = videosPerLoad;
            hasMoreVideos = true;
            
            var initialVideos = await VideoService.GetRandomVideosAsync(videosPerLoad);
            randomVideos.AddRange(initialVideos);
            
            show = false;
            activeTab = "giscus";
        }
        finally
        {
            await StateService.SetLoadingStateAsync(false);
        }
    }

    private async Task LoadMoreVideos()
    {
        if (isLoadingMore || !hasMoreVideos) return;
        
        isLoadingMore = true;
        StateHasChanged();
        
        try
        {
            var allVideos = await VideoService.GetAllVideosAsync();
            var allVideosList = allVideos.ToList();
            
            if (currentVideoCount >= allVideosList.Count)
            {
                hasMoreVideos = false;
                return;
            }
            
            var remainingVideos = allVideosList
                .Where(v => v.Id != currentVideo?.Id && !randomVideos.Any(rv => rv.Id == v.Id))
                .ToList();
            
            var videosToAdd = remainingVideos
                .OrderBy(x => Guid.NewGuid())
                .Take(videosPerLoad)
                .ToList();
            
            randomVideos.AddRange(videosToAdd);
            currentVideoCount += videosToAdd.Count;
            
            if (currentVideoCount >= allVideosList.Count || videosToAdd.Count < videosPerLoad)
            {
                hasMoreVideos = false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading more videos: {ex.Message}");
        }
        finally
        {
            isLoadingMore = false;
            StateHasChanged();
        }
    }

    public void Dispose()
    {
        StateService.OnStateChanged -= StateHasChanged;
    }
    
    private string GetRandomViews()
    {
        var random = new Random();
        var views = random.Next(100000, 10000000);
        if (views >= 1000000)
        {
            return $"{views / 1000000:F1}M";
        }
        else if (views >= 1000)
        {
            return $"{views / 1000:F1}K";
        }
        return views.ToString();
    }
    
    private string GetRandomTimeAgo()
    {
        var random = new Random();
        var days = random.Next(1, 365);
        if (days < 7)
            return $"{days} days ago";
        else if (days < 30)
            return $"{days / 7} weeks ago";
        else if (days < 365)
            return $"{days / 30} months ago";
        else
            return $"{days / 365} years ago";
    }
    
    private string GetRandomLikes()
    {
        var random = new Random();
        var likes = random.Next(100, 50000);
        if (likes >= 1000)
        {
            return $"{likes / 1000:F1}K";
        }
        return likes.ToString();
    }
    
    private string GetRandomVideos()
    {
        var random = new Random();
        var videos = random.Next(50, 500);
        return $"{videos}";
    }
    
    private string GetRandomComments()
    {
        var random = new Random();
        var comments = random.Next(10, 1000);
        if (comments >= 1000)
        {
            return $"{comments / 1000:F1}K comments";
        }
        return $"{comments} comments";
    }
    
    private string GetEmbeddableUrl(string originalUrl)
    {
        if (string.IsNullOrEmpty(originalUrl))
            return "";
            
        if (originalUrl.Contains("drive.google.com"))
        {
            var fileId = ExtractFileIdFromGoogleDriveUrl(originalUrl);
            if (!string.IsNullOrEmpty(fileId))
            {
                return $"https://drive.google.com/file/d/{fileId}/preview";
            }
        }
        
        return originalUrl;
    }
    
    private string ExtractFileIdFromGoogleDriveUrl(string url)
    {
        try
        {
            if (url.Contains("/file/d/"))
            {
                var startIndex = url.IndexOf("/file/d/") + 8;
                var endIndex = url.IndexOf("/", startIndex);
                if (endIndex == -1) endIndex = url.Length;
                return url.Substring(startIndex, endIndex - startIndex);
            }
            else if (url.Contains("id="))
            {
                var startIndex = url.IndexOf("id=") + 3;
                var endIndex = url.IndexOf("&", startIndex);
                if (endIndex == -1) endIndex = url.Length;
                return url.Substring(startIndex, endIndex - startIndex);
            }
        }
        catch
        {
        }
        
        return "";
    }
}


