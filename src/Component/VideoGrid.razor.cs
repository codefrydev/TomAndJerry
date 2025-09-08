using TomAndJerry.Services;

namespace TomAndJerry.Component;

public partial class VideoGrid : BaseComponent
{
    protected override async Task OnComponentInitializedAsync()
    {
        if (!AppService.IsInitialized)
        {
            await AppService.InitializeApplicationAsync();
        }
    }
}
