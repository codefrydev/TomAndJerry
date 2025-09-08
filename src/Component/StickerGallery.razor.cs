using Microsoft.AspNetCore.Components;
using TomAndJerry.Services;
using TomAndJerry.Model;

namespace TomAndJerry.Component;

public partial class StickerGallery : BaseComponent
{
    [Parameter] public int MaxStickers { get; set; } = 24;
    [Parameter] public string Category { get; set; } = string.Empty;
    [Parameter] public EventCallback<Sticker> OnStickerSelected { get; set; }

    private List<Sticker> stickers = new();
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadStickersAsync();
    }

    private async Task LoadStickersAsync()
    {
        isLoading = true;
        StateHasChanged();

        try
        {
            if (!string.IsNullOrEmpty(Category))
            {
                var allStickers = await StickerService.GetAllStickersAsync();
                stickers = allStickers.Where(s => s.Category.Equals(Category, StringComparison.OrdinalIgnoreCase))
                                    .Take(MaxStickers)
                                    .ToList();
            }
            else
            {
                stickers = await StickerService.GetRandomStickersAsync(MaxStickers);
            }
        }
        catch (Exception)
        {
            // Handle error - could log or show error message
            stickers = new List<Sticker>();
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    private async Task OnStickerClick(Sticker sticker)
    {
        if (OnStickerSelected.HasDelegate)
        {
            await OnStickerSelected.InvokeAsync(sticker);
        }
    }

    public async Task RefreshStickersAsync()
    {
        await LoadStickersAsync();
    }
}
