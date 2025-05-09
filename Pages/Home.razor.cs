using TomAndJerry.Model;

namespace TomAndJerry.Pages;

public partial class Home
{
    private void GoToPage(Video video)
    {
        Nav.NavigateTo($"playmedia/{video.Id}");
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