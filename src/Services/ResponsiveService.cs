using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TomAndJerry.Services
{
    public class ResponsiveService : IResponsiveService, IDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private bool _isMobile = false;
        private bool _isTablet = false;
        private bool _isDesktop = false;
        private DotNetObjectReference<ResponsiveService>? _dotNetRef;

        public ResponsiveService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public bool IsMobile => _isMobile;
        public bool IsTablet => _isTablet;
        public bool IsDesktop => _isDesktop;

        public event Action? OnBreakpointChanged;

        public async Task InitializeAsync()
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync("responsiveService.initialize", _dotNetRef);
        }

        public async Task<bool> IsMobileAsync()
        {
            return await _jsRuntime.InvokeAsync<bool>("responsiveService.isMobile");
        }

        public async Task<bool> IsTabletAsync()
        {
            return await _jsRuntime.InvokeAsync<bool>("responsiveService.isTablet");
        }

        public async Task<bool> IsDesktopAsync()
        {
            return await _jsRuntime.InvokeAsync<bool>("responsiveService.isDesktop");
        }

        [JSInvokable]
        public void OnBreakpointChange(bool isMobile, bool isTablet, bool isDesktop)
        {
            _isMobile = isMobile;
            _isTablet = isTablet;
            _isDesktop = isDesktop;
            OnBreakpointChanged?.Invoke();
        }

        public void Dispose()
        {
            _dotNetRef?.Dispose();
        }
    }
}
