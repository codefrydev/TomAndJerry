using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using TomAndJerry.Services;
using TomAndJerry.Model;
using TomAndJerry.Component;
using TomAndJerry.Utils;

namespace TomAndJerry.Pages;

public partial class Home : IDisposable
{
    private string heroSearchTerm = string.Empty;
    private string activeFilter = "All";
    private IEnumerable<Video> filteredVideos = Enumerable.Empty<Video>();
    private Snackbar snackbar = new();

    public void GoTOPage(Video video)
    {
        nav.NavigateTo($"playmedia/{video.Id}");
    }

    private void PerformHeroSearch()
    {
        if (!string.IsNullOrEmpty(heroSearchTerm))
        {
            nav.NavigateTo($"Search/{Uri.EscapeDataString(heroSearchTerm)}");
        }
    }

    private void HandleHeroSearchKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            PerformHeroSearch();
        }
    }

    private async Task ScrollToSection(string sectionId)
    {
        await JSRuntime.InvokeVoidAsync("scrollToElement", sectionId);
    }

    private async Task ScrollToFeatured()
    {
        await ScrollToSection("featured");
    }

    private async Task ScrollToAllEpisodes()
    {
        await ScrollToSection("all-episodes");
    }

    private async Task ScrollToStickerGallery()
    {
        await ScrollToSection("sticker-gallery");
    }

    private void GoToQuiz()
    {
        nav.NavigateTo("quiz");
    }

    private void RefreshStickers()
    {
        // This will trigger a refresh of the sticker gallery
        InvokeAsync(StateHasChanged);
    }

    private void ShowAllStickers()
    {
        // Navigate to the dedicated stickers page
        nav.NavigateTo("stickers");
    }

    private async Task OnStickerSelected(Sticker sticker)
    {
        // Handle sticker selection - could show a modal, navigate, or perform an action
        await snackbar.ShowAsync("Sticker Selected!", $"You selected: {sticker.DisplayName}!", "🎭", SnackbarType.Success, 4000);
    }

    private async Task ShowCharacterInfo(string character)
    {
        string info = character == "Tom" 
            ? "Tom is a blue-grey cat who lives in the house with his owner. Despite his best efforts, he can never catch Jerry, but that doesn't stop him from trying! Tom is known for his elaborate schemes and comedic failures."
            : "Jerry is a small brown mouse who lives in the house. He's incredibly clever and always manages to outsmart Tom with his quick thinking and resourcefulness. Jerry loves cheese and enjoys playing tricks on Tom.";
        
        string icon = character == "Tom" ? "🐱" : "🐭";
        await snackbar.ShowAsync($"{character} Info", info, icon, SnackbarType.Info, 8000);
    }

    private async Task ShowRandomFact()
    {
        var fact = RandomFactsService.GetRandomFact();
        await snackbar.ShowAsync("🎭 Fun Fact", fact, "💡", SnackbarType.Info, 6000);
    }

    private async Task RefreshFeaturedEpisodes()
    {
        await StateService.RefreshFeaturedVideosAsync(StateService.CurrentVideos, 10);
    }

    private void ApplyFilter(string filter)
    {
        activeFilter = filter;
        
        if (!StateService.CurrentVideos.Any())
        {
            filteredVideos = Enumerable.Empty<Video>();
            return;
        }

        var videos = StateService.CurrentVideos.ToList();
        
        switch (filter)
        {
            case "All":
                // Sequential order (by ID)
                filteredVideos = videos.OrderBy(v => int.Parse(v.Id)).ToList();
                break;
            case "Classic":
                // Random order
                var random = new Random();
                filteredVideos = videos.OrderBy(x => random.Next()).ToList();
                break;
            case "Modern":
                // Inverse order (newest first)
                filteredVideos = videos.OrderByDescending(v => int.Parse(v.Id)).ToList();
                break;
            default:
                filteredVideos = videos;
                break;
        }
        
        StateHasChanged();
    }

    private string GetFilterButtonClass(string filter)
    {
        return activeFilter == filter
            ? "px-4 sm:px-6 py-2 sm:py-3 text-xs sm:text-sm bg-tom-blue text-white rounded-2xl font-bold font-comic hover:bg-tom-dark-blue transition-colors cartoon-rounded"
            : "px-4 sm:px-6 py-2 sm:py-3 text-xs sm:text-sm bg-soft-blue text-amber-800 rounded-2xl font-bold font-comic hover:bg-cartoon-yellow transition-colors border-2 border-amber-800 cartoon-rounded";
    }

    protected override async Task OnInitializedAsync()
    {
        StateService.OnStateChanged += StateHasChanged;
        
        await StateService.SetLoadingStateAsync(true);
        
        try
        {
            await VideoService.InitializeAsync();
            var videos = await VideoService.GetAllVideosAsync();
            await StateService.SetVideosAsync(videos);
            
            // Set random featured videos
            await StateService.RefreshFeaturedVideosAsync(videos, 10);
            
            // Apply initial filter (All - sequential order)
            ApplyFilter("All");
        }
        finally
        {
            await StateService.SetLoadingStateAsync(false);
        }
    }

    public void Dispose()
    {
        StateService.OnStateChanged -= StateHasChanged;
    }
}
