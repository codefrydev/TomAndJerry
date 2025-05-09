using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TomAndJerry.Model;

namespace TomAndJerry.Pages;

public partial class PlayMedia
{
    private bool _show = true;
    [Parameter] public string VideId { get; set; } = string.Empty;

    private async Task GoToPage(Video video)
    {
        await LoadGiscud(video.CommentName);
        Nav.NavigateTo($"playmedia/{video.Id}");
    }

    private async Task LoadGiscud(string id)
    {
        await Js.InvokeVoidAsync("loadDisqus", Nav.Uri, id);
    }

    private void ShowComment()
    {
        _show = false;
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        Datas.OnChange += StateHasChanged;
        //await Datas.InitializeAsync();
    }

    public void Dispose()
    {
        Datas.OnChange -= StateHasChanged;
    }
}