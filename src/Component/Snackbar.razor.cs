using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TomAndJerry.Component;

public partial class Snackbar : IDisposable
{
    [Parameter] public string Title { get; set; } = "";
    [Parameter] public string Message { get; set; } = "";
    [Parameter] public string Icon { get; set; } = "";
    [Parameter] public SnackbarType Type { get; set; } = SnackbarType.Info;
    [Parameter] public int Duration { get; set; } = 5000; // 5 seconds
    [Parameter] public EventCallback OnClose { get; set; }

    private bool IsVisible { get; set; } = false;
    private Timer? _timer;

    public async Task ShowAsync(string title, string message, string icon = "", SnackbarType type = SnackbarType.Info, int duration = 5000)
    {
        Title = title;
        Message = message;
        Icon = icon;
        Type = type;
        Duration = duration;
        IsVisible = true;
        
        StateHasChanged();
        
        // Auto-hide after duration
        if (Duration > 0)
        {
            _timer?.Dispose();
            _timer = new Timer(async _ => await InvokeAsync(Hide), null, Duration, Timeout.Infinite);
        }
        
        // Trigger slide-in animation
        await Task.Delay(50);
        await JSRuntime.InvokeVoidAsync("showSnackbar");
    }

    public async Task Hide()
    {
        if (!IsVisible) return;
        
        await JSRuntime.InvokeVoidAsync("hideSnackbar");
        
        await Task.Delay(300); // Wait for animation to complete
        IsVisible = false;
        _timer?.Dispose();
        _timer = null;
        
        StateHasChanged();
        await OnClose.InvokeAsync();
    }

    private string GetSnackbarClass()
    {
        return Type switch
        {
            SnackbarType.Success => "snackbar-success",
            SnackbarType.Error => "snackbar-error",
            SnackbarType.Warning => "snackbar-warning",
            SnackbarType.Info => "snackbar-info",
            _ => "snackbar-info"
        };
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}


