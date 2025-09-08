using Microsoft.AspNetCore.Components;
using TomAndJerry.Model;

namespace TomAndJerry.Pages;

public partial class Search : IDisposable
{
    [Parameter] public string searchterm { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        StateService.OnStateChanged += StateHasChanged;
        
        await StateService.SetLoadingStateAsync(true);
        
        try
        {
            await VideoService.InitializeAsync();
            
            if (string.IsNullOrWhiteSpace(searchterm))
            {
                var allVideos = await VideoService.GetAllVideosAsync();
                await StateService.SetFilteredVideosAsync(allVideos);
                await StateService.SetSearchTermAsync("");
            }
            else
            {
                var searchResults = await SearchService.SearchAsync(searchterm);
                await StateService.SetFilteredVideosAsync(searchResults);
                await StateService.SetSearchTermAsync(searchterm);
            }
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

    public void GoTOPage(Video video)
    {
        nav.NavigateTo($"playmedia/{video.Id}");
    }
}


