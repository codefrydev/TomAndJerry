using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TomAndJerry.Component;

public partial class LottieAnimation : IDisposable
{
    [Parameter] public string AnimationPath { get; set; } = "";
    [Parameter] public string Width { get; set; } = "300px";
    [Parameter] public string Height { get; set; } = "300px";
    [Parameter] public bool Loop { get; set; } = true;
    [Parameter] public bool Autoplay { get; set; } = true;
    [Parameter] public string Renderer { get; set; } = "svg";
    [Parameter] public string AdditionalStyles { get; set; } = "";
    [Parameter] public string CssClass { get; set; } = "";

    private string ContainerId { get; set; } = "";
    private IJSObjectReference? lottieModule;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !string.IsNullOrEmpty(AnimationPath))
        {
            ContainerId = $"lottie-container-{Guid.NewGuid():N}";
            await LoadLottieAnimation();
        }
    }

    private async Task LoadLottieAnimation()
    {
        try
        {
            lottieModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/lottie-interop.js");
            await lottieModule.InvokeVoidAsync("loadLottieAnimation", new
            {
                containerId = ContainerId,
                path = AnimationPath,
                renderer = Renderer,
                loop = Loop,
                autoplay = Autoplay
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading Lottie animation: {ex.Message}");
        }
    }

    public async Task Play()
    {
        if (lottieModule != null)
        {
            await lottieModule.InvokeVoidAsync("playAnimation", ContainerId);
        }
    }

    public async Task Pause()
    {
        if (lottieModule != null)
        {
            await lottieModule.InvokeVoidAsync("pauseAnimation", ContainerId);
        }
    }

    public async Task Stop()
    {
        if (lottieModule != null)
        {
            await lottieModule.InvokeVoidAsync("stopAnimation", ContainerId);
        }
    }

    public async Task SetSpeed(float speed)
    {
        if (lottieModule != null)
        {
            await lottieModule.InvokeVoidAsync("setAnimationSpeed", ContainerId, speed);
        }
    }

    public void Dispose()
    {
        lottieModule?.DisposeAsync();
    }
}
