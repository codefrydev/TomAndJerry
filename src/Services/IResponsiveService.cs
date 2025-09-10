using Microsoft.AspNetCore.Components;

namespace TomAndJerry.Services
{
    public interface IResponsiveService
    {
        bool IsMobile { get; }
        bool IsTablet { get; }
        bool IsDesktop { get; }
        event Action? OnBreakpointChanged;
        Task InitializeAsync();
        Task<bool> IsMobileAsync();
        Task<bool> IsTabletAsync();
        Task<bool> IsDesktopAsync();
    }
}
