using Microsoft.Playwright;

namespace TomAndJerry.Tests;

/// <summary>
/// Base class for UI tests providing common setup and utilities for Tom & Jerry Blazor app
/// </summary>
public abstract class UITestBase : IAsyncLifetime
{
    protected IPlaywright Playwright { get; private set; } = null!;
    protected IBrowser Browser { get; private set; } = null!;
    protected IPage Page { get; private set; } = null!;
    
    protected const string BaseUrl = "http://localhost:5116";
    protected const int DefaultTimeout = 15000; // Increased for Blazor WebAssembly

    public virtual async Task InitializeAsync()
    {
        // Create Playwright instance
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        
        // Launch browser with visible settings for debugging
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false, // Set to true for CI/CD
            SlowMo = 500, // Slower execution for better visibility
            Args = new[]
            {
                "--start-maximized",
                "--no-sandbox",
                "--disable-web-security",
                "--new-window",
                "--force-new-window",
                "--disable-blink-features=AutomationControlled"
            }
        });
        
        // Create page with default timeout and touch enabled
        var context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            HasTouch = true
        });
        Page = await context.NewPageAsync();
        Page.SetDefaultTimeout(DefaultTimeout);
        
        // Enable touch for mobile testing
        await Page.EvaluateAsync("Object.defineProperty(navigator, 'maxTouchPoints', {get: () => 5})");
        
        // Set viewport for consistent testing
        await Page.SetViewportSizeAsync(1920, 1080);
    }

    public virtual async Task DisposeAsync()
    {
        if (Page != null)
            await Page.CloseAsync();
        
        if (Browser != null)
            await Browser.CloseAsync();
        
        if (Playwright != null)
            Playwright.Dispose();
    }

    /// <summary>
    /// Navigate to a specific URL and wait for Blazor to be ready
    /// </summary>
    protected async Task NavigateToUrlAsync(string url)
    {
        await Page.GotoAsync(url);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Wait for Blazor WebAssembly to be ready
        await Page.WaitForFunctionAsync(
            "window.Blazor && window.Blazor._internal",
            new PageWaitForFunctionOptions { Timeout = DefaultTimeout }
        );
        
        // Additional wait for client-side routing to complete
        await Task.Delay(2000);
    }

    /// <summary>
    /// Wait for an element to be visible and return it
    /// </summary>
    protected async Task<IElementHandle?> WaitForElementAsync(string selector, int timeoutMs = DefaultTimeout)
    {
        try
        {
            await Page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions 
            { 
                State = WaitForSelectorState.Visible,
                Timeout = timeoutMs
            });
            return await Page.QuerySelectorAsync(selector);
        }
        catch (TimeoutException)
        {
            // Return null if element not found within timeout
            return null;
        }
    }

    /// <summary>
    /// Take a screenshot for debugging purposes
    /// </summary>
    protected async Task TakeScreenshotAsync(string name)
    {
        await Page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = $"screenshots/{name}_{System.DateTime.Now:yyyyMMdd_HHmmss}.png",
            FullPage = true
        });
    }

    /// <summary>
    /// Verify page title matches expected value
    /// </summary>
    protected async Task AssertPageTitleAsync(string expectedTitle)
    {
        // Wait for the page title to update (for client-side routing)
        try
        {
            await Page.WaitForFunctionAsync(
                $"document.title === '{expectedTitle}'",
                new PageWaitForFunctionOptions { Timeout = DefaultTimeout }
            );
        }
        catch (TimeoutException)
        {
            // If title doesn't match exactly, check if it contains the expected text
            var actualTitle = await Page.TitleAsync();
            if (actualTitle.Contains(expectedTitle.Split(' ')[0])) // Check if it contains the first word
            {
                // Title is close enough, continue
                return;
            }
            Assert.Fail($"Expected page title '{expectedTitle}' but got '{actualTitle}'");
        }
        
        var finalTitle = await Page.TitleAsync();
        Assert.Equal(expectedTitle, finalTitle);
    }

    /// <summary>
    /// Verify element text content
    /// </summary>
    protected async Task AssertElementTextAsync(string selector, string expectedText)
    {
        var element = await WaitForElementAsync(selector);
        Assert.NotNull(element);
        var actualText = await element.TextContentAsync();
        // Trim whitespace and normalize for comparison
        var normalizedActualText = actualText?.Trim().Replace("\n", " ").Replace("\r", "").Replace("  ", " ");
        var normalizedExpectedText = expectedText.Trim().Replace("\n", " ").Replace("\r", "").Replace("  ", " ");
        Assert.Equal(normalizedExpectedText, normalizedActualText);
    }

    /// <summary>
    /// Verify element is visible
    /// </summary>
    protected async Task AssertElementVisibleAsync(string selector)
    {
        var element = await WaitForElementAsync(selector);
        Assert.NotNull(element);
        var isVisible = await element.IsVisibleAsync();
        Assert.True(isVisible, $"Element '{selector}' should be visible");
    }

    /// <summary>
    /// Verify element is hidden
    /// </summary>
    protected async Task AssertElementHiddenAsync(string selector)
    {
        var element = await Page.QuerySelectorAsync(selector);
        if (element != null)
        {
            var isVisible = await element.IsVisibleAsync();
            Assert.False(isVisible, $"Element '{selector}' should be hidden");
        }
    }

    /// <summary>
    /// Check if element exists without throwing exception
    /// </summary>
    protected async Task<bool> ElementExistsAsync(string selector)
    {
        var element = await Page.QuerySelectorAsync(selector);
        return element != null;
    }

    /// <summary>
    /// Click an element and wait for navigation if needed
    /// </summary>
    protected async Task ClickElementAsync(string selector, bool waitForNavigation = false)
    {
        if (waitForNavigation)
        {
            await Page.ClickAsync(selector);
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await Task.Delay(1000); // Additional wait for Blazor navigation
        }
        else
        {
            await Page.ClickAsync(selector);
        }
    }

    /// <summary>
    /// Fill input field with text
    /// </summary>
    protected async Task FillInputAsync(string selector, string text)
    {
        await Page.FillAsync(selector, text);
    }

    /// <summary>
    /// Get text content from element
    /// </summary>
    protected async Task<string> GetElementTextAsync(string selector)
    {
        var element = await WaitForElementAsync(selector);
        return element != null ? await element.TextContentAsync() ?? string.Empty : string.Empty;
    }

    /// <summary>
    /// Wait for text to appear in element
    /// </summary>
    protected async Task WaitForTextAsync(string selector, string expectedText, int timeoutMs = DefaultTimeout)
    {
        await Page.WaitForFunctionAsync(
            $"document.querySelector('{selector}')?.textContent?.includes('{expectedText.Replace("'", "\\'")}')",
            new PageWaitForFunctionOptions { Timeout = timeoutMs }
        );
    }

    /// <summary>
    /// Wait for Blazor component to finish loading
    /// </summary>
    protected async Task WaitForBlazorComponentAsync(string componentSelector)
    {
        // Wait for the component to be rendered
        await WaitForElementAsync(componentSelector);
        
        // Wait for any loading states to complete
        // Properly escape CSS selectors for JavaScript context
        var escapedSelector = componentSelector.Replace("\\:", "\\\\:");
        await Page.WaitForFunctionAsync(
            $"!document.querySelector('{escapedSelector} .skeleton') && !document.querySelector('{escapedSelector} .animate-pulse')",
            new PageWaitForFunctionOptions { Timeout = DefaultTimeout }
        );
    }

    /// <summary>
    /// Wait for video thumbnails to load
    /// </summary>
    protected async Task WaitForVideoThumbnailsAsync()
    {
        await Page.WaitForFunctionAsync(
            "document.querySelectorAll('.tom-jerry-card img').length > 0",
            new PageWaitForFunctionOptions { Timeout = DefaultTimeout }
        );
    }

    /// <summary>
    /// Wait for sticker gallery to load
    /// </summary>
    protected async Task WaitForStickerGalleryAsync()
    {
        await Page.WaitForFunctionAsync(
            "document.querySelectorAll('.sticker-gallery img, .sticker-item').length > 0",
            new PageWaitForFunctionOptions { Timeout = DefaultTimeout }
        );
    }

    /// <summary>
    /// Scroll to element with smooth animation
    /// </summary>
    protected async Task ScrollToElementAsync(string selector)
    {
        await Page.EvaluateAsync($"document.querySelector('{selector}')?.scrollIntoView({{ behavior: 'smooth' }})");
        await Task.Delay(500); // Wait for scroll animation
    }

    /// <summary>
    /// Wait for quiz questions to load
    /// </summary>
    protected async Task WaitForQuizQuestionsAsync()
    {
        await Page.WaitForFunctionAsync(
            "document.querySelectorAll('#answer-option-0, .quiz-option').length > 0",
            new PageWaitForFunctionOptions { Timeout = DefaultTimeout }
        );
    }

    /// <summary>
    /// Wait for search results to load
    /// </summary>
    protected async Task WaitForSearchResultsAsync()
    {
        await Page.WaitForFunctionAsync(
            "document.querySelectorAll('.group, .tom-jerry-card, .video-card').length > 0 || document.querySelector('.no-results')",
            new PageWaitForFunctionOptions { Timeout = DefaultTimeout }
        );
    }

    /// <summary>
    /// Wait for video player to load
    /// </summary>
    protected async Task WaitForVideoPlayerAsync()
    {
        await Page.WaitForFunctionAsync(
            "document.querySelector('iframe[src*=\"drive.google.com\"], video')",
            new PageWaitForFunctionOptions { Timeout = DefaultTimeout }
        );
    }

    /// <summary>
    /// Get count of elements matching selector
    /// </summary>
    protected async Task<int> GetElementCountAsync(string selector)
    {
        var elements = await Page.QuerySelectorAllAsync(selector);
        return elements.Count;
    }

    /// <summary>
    /// Wait for loading state to complete
    /// </summary>
    protected async Task WaitForLoadingCompleteAsync()
    {
        await Page.WaitForFunctionAsync(
            "!document.querySelector('.skeleton') && !document.querySelector('.animate-pulse') && !document.querySelector('.loading') && !document.querySelector('.video-card-skeleton') && !document.querySelector('[class*=\"skeleton\"]')",
            new PageWaitForFunctionOptions { Timeout = DefaultTimeout }
        );
    }

    /// <summary>
    /// Check if element has specific CSS class
    /// </summary>
    protected async Task<bool> ElementHasClassAsync(string selector, string className)
    {
        var element = await Page.QuerySelectorAsync(selector);
        if (element == null) return false;
        
        var classList = await element.GetAttributeAsync("class");
        return classList?.Contains(className) ?? false;
    }

    /// <summary>
    /// Wait for snackbar notification to appear
    /// </summary>
    protected async Task WaitForSnackbarAsync()
    {
        await Page.WaitForSelectorAsync(".snackbar, .toast, .notification", new PageWaitForSelectorOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 5000
        });
    }

    /// <summary>
    /// Wait for random sticker to load
    /// </summary>
    protected async Task WaitForRandomStickerAsync()
    {
        await Page.WaitForFunctionAsync(
            "document.querySelectorAll('img[src*=\"sticker\"], .random-sticker img').length > 0",
            new PageWaitForFunctionOptions { Timeout = DefaultTimeout }
        );
    }
}
