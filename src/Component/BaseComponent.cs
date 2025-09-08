using TomAndJerry.Services;
using Microsoft.AspNetCore.Components;

namespace TomAndJerry.Component;

public abstract class BaseComponent : ComponentBase, IDisposable
{
    [Inject] protected IStateService StateService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        StateService.OnStateChanged += StateHasChanged;
        await OnComponentInitializedAsync();
    }

    protected virtual async Task OnComponentInitializedAsync()
    {
        await Task.CompletedTask;
    }

    public virtual void Dispose()
    {
        StateService.OnStateChanged -= StateHasChanged;
    }
}
