using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using TomAndJerry.Services;

namespace TomAndJerry.Component;

public partial class RealTimeSearch : IDisposable
{
    private string searchTerm = string.Empty;
    private IEnumerable<string> searchSuggestions = Enumerable.Empty<string>();
    private IEnumerable<TomAndJerry.Model.Video> searchResults = Enumerable.Empty<TomAndJerry.Model.Video>();
    private bool showSuggestions = false;
    private Timer? searchTimer;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("addClickOutsideHandler", DotNetObjectReference.Create(this));
        }
    }

    protected override void OnInitialized()
    {
        SearchService.OnSearchResultsChanged += OnSearchResultsChanged;
        
        // Initialize search timer for debouncing (reduced to 150ms for better responsiveness)
        searchTimer = new Timer(async _ => await DebouncedSearch(), null, Timeout.Infinite, Timeout.Infinite);
    }

    private void OnSearchResultsChanged(IEnumerable<TomAndJerry.Model.Video> results)
    {
        searchResults = results;
        
        // Clear suggestions when we have search results
        if (searchResults.Any())
        {
            searchSuggestions = Enumerable.Empty<string>();
            showSuggestions = false;
        }
        
        InvokeAsync(StateHasChanged);
    }

    private async Task DebouncedSearch()
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            searchSuggestions = Enumerable.Empty<string>();
            searchResults = Enumerable.Empty<TomAndJerry.Model.Video>();
            showSuggestions = false;
            await InvokeAsync(StateHasChanged);
            return;
        }

        // Perform immediate search for preview
        var immediateResults = await VideoService.SearchVideosAsync(searchTerm);
        searchResults = immediateResults;
        
        // Only show suggestions if no search results found and search term is long enough
        if (!searchResults.Any() && searchTerm.Length >= 2)
        {
            searchSuggestions = await SearchService.GetSearchSuggestionsAsync(searchTerm);
            showSuggestions = searchSuggestions.Any();
        }
        else
        {
            // Clear suggestions when we have results or search term is too short
            searchSuggestions = Enumerable.Empty<string>();
            showSuggestions = false;
        }
        
        // Also trigger the debounced search for suggestions
        await SearchService.SearchAsync(searchTerm);
        
        await InvokeAsync(StateHasChanged);
    }

    private void HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            PerformSearch();
        }
        else if (e.Key == "Escape")
        {
            CloseDropdown();
        }
        else
        {
            // Debounce search input (reduced to 150ms for better responsiveness)
            searchTimer?.Change(150, Timeout.Infinite);
        }
    }

    private void PerformSearch()
    {
        if (!string.IsNullOrEmpty(searchTerm))
        {
            CloseDropdown();
            NavigationManager.NavigateTo($"Search/{Uri.EscapeDataString(searchTerm)}");
        }
    }

    private void SelectSuggestion(string suggestion)
    {
        searchTerm = suggestion;
        CloseDropdown();
        PerformSearch();
    }

    private void NavigateToSearchResults()
    {
        if (!string.IsNullOrEmpty(searchTerm))
        {
            CloseDropdown();
            NavigationManager.NavigateTo($"Search/{Uri.EscapeDataString(searchTerm)}");
        }
    }

    private void NavigateToVideo(TomAndJerry.Model.Video video)
    {
        CloseDropdown();
        NavigationManager.NavigateTo($"playmedia/{video.Id}");
    }

    private void CloseDropdown()
    {
        showSuggestions = false;
        searchSuggestions = Enumerable.Empty<string>();
        searchResults = Enumerable.Empty<TomAndJerry.Model.Video>();
        InvokeAsync(StateHasChanged);
    }

    private string GetCleanTitle(string description)
    {
        return string.Join(" ", description.Split(".").Where(x => x != "mkv").Select(x => x));
    }

    [JSInvokable]
    public void OnClickOutside()
    {
        CloseDropdown();
    }

    public void Dispose()
    {
        SearchService.OnSearchResultsChanged -= OnSearchResultsChanged;
        searchTimer?.Dispose();
    }
}


