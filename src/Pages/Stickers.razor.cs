using TomAndJerry.Services;
using TomAndJerry.Model;
using TomAndJerry.Component;
using TomAndJerry.Utils;

namespace TomAndJerry.Pages;

public partial class Stickers
{
    private List<Sticker> allStickers = new();
    private List<Sticker> filteredStickers = new();
    private List<string> availableCategories = new();
    private string selectedCategory = string.Empty;
    private int currentDisplayCount = 12;
    private Snackbar snackbar = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadStickersAsync();
    }

    private async Task LoadStickersAsync()
    {
        allStickers = await StickerService.GetAllStickersAsync();
        availableCategories = allStickers.Select(s => s.Category).Distinct().OrderBy(c => c).ToList();
        FilterByCategory(string.Empty);
    }

    private void FilterByCategory(string category)
    {
        selectedCategory = category;
        if (string.IsNullOrEmpty(category))
        {
            filteredStickers = allStickers.ToList();
        }
        else
        {
            filteredStickers = allStickers.Where(s => s.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        currentDisplayCount = 12;
        StateHasChanged();
    }

    private async Task LoadMoreStickers()
    {
        currentDisplayCount = Math.Min(currentDisplayCount + 12, filteredStickers.Count());
        await InvokeAsync(StateHasChanged);
    }

    private async Task OnStickerSelected(Sticker sticker)
    {
        var fact = RandomFactsService.GetRandomFact();
        await snackbar.ShowAsync("🎭 Fun Fact", fact, "💡", SnackbarType.Info, 7000);
    }

    private void GoBack()
    {
        Nav.NavigateTo("");
    }
}
