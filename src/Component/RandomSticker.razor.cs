using Microsoft.AspNetCore.Components;
using TomAndJerry.Services;
using TomAndJerry.Model;

namespace TomAndJerry.Component;

public partial class RandomSticker : BaseComponent
{
    [Parameter] public string CssClass { get; set; } = string.Empty;
    [Parameter] public string ImageCssClass { get; set; } = string.Empty;
    [Parameter] public string Style { get; set; } = string.Empty;
    [Parameter] public string Category { get; set; } = string.Empty;
    [Parameter] public bool AutoRefresh { get; set; } = false;
    [Parameter] public int RefreshIntervalSeconds { get; set; } = 30;

    private Sticker? currentSticker;
    private Timer? refreshTimer;

    protected override async Task OnInitializedAsync()
    {
        await LoadRandomStickerAsync();
        
        if (AutoRefresh)
        {
            refreshTimer = new Timer(async _ => await LoadRandomStickerAsync(), null, 
                TimeSpan.FromSeconds(RefreshIntervalSeconds), 
                TimeSpan.FromSeconds(RefreshIntervalSeconds));
        }
    }

    private async Task LoadRandomStickerAsync()
    {
        try
        {
            if (!string.IsNullOrEmpty(Category))
            {
                var allStickers = await StickerService.GetAllStickersAsync();
                var categoryStickers = allStickers.Where(s => s.Category.Equals(Category, StringComparison.OrdinalIgnoreCase)).ToList();
                if (categoryStickers.Any())
                {
                    var random = new Random();
                    currentSticker = categoryStickers[random.Next(categoryStickers.Count)];
                }
                else
                {
                    currentSticker = await StickerService.GetRandomStickerAsync();
                }
            }
            else
            {
                currentSticker = await StickerService.GetRandomStickerAsync();
            }
            
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception)
        {
            // Fallback to null, which will show Tom.png
            currentSticker = null;
        }
    }

    public async Task RefreshStickerAsync()
    {
        await LoadRandomStickerAsync();
    }

    public new void Dispose()
    {
        refreshTimer?.Dispose();
        base.Dispose();
    }
}
