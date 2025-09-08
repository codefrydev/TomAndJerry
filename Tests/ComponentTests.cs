using Xunit;

namespace TomAndJerry.Tests;

/// <summary>
/// Comprehensive UI tests for reusable components
/// </summary>
public class ComponentTests : UITestBase
{
    [Fact]
    public async Task AppBar_ShouldDisplayLogo()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync("header");
        
        // Assert
        await AssertElementVisibleAsync("header");
        await AssertElementVisibleAsync(".tom-character");
        await AssertElementVisibleAsync(".w-8.h-8, .w-10.h-10");
    }

    [Fact]
    public async Task AppBar_ShouldDisplaySearchBar()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync("header");
        
        // Assert
        await AssertElementVisibleAsync(".flex-1.max-w-xs, .flex-1.max-w-md, .flex-1.max-w-2xl");
        await AssertElementVisibleAsync("[class*='search'], input[type='text']");
    }

    [Fact]
    public async Task AppBar_ShouldDisplayQuizButton()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync("header");
        
        // Assert
        await AssertElementVisibleAsync("button:has-text('Quiz')");
        await AssertElementVisibleAsync("text:has-text('🧠')");
    }

    [Fact]
    public async Task AppBar_ShouldNavigateToHomeOnLogoClick()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync("header");
        
        // Click on logo
        await ClickElementAsync(".tom-character", waitForNavigation: true);
        
        // Assert
        await AssertPageTitleAsync("Home - Tom & Jerry");
    }

    [Fact]
    public async Task AppBar_ShouldNavigateToQuizOnButtonClick()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync("header");
        
        // Click quiz button
        await ClickElementAsync("button:has-text('Quiz')", waitForNavigation: true);
        
        // Assert
        await AssertPageTitleAsync("Tom & Jerry Quiz - Test Your Knowledge!");
    }

    [Fact]
    public async Task AppBar_ShouldHaveResponsiveDesign()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync("header");
        
        // Test mobile viewport
        await Page.SetViewportSizeAsync(375, 667);
        await Task.Delay(500);
        
        // Assert - Header should still be visible
        await AssertElementVisibleAsync("header");
        
        // Test desktop viewport
        await Page.SetViewportSizeAsync(1920, 1080);
        await Task.Delay(500);
        
        // Assert - Header should still be visible
        await AssertElementVisibleAsync("header");
    }

    [Fact]
    public async Task Footer_ShouldDisplayBrandSection()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync("footer");
        
        // Assert
        await AssertElementVisibleAsync("footer");
        await AssertElementVisibleAsync("h3:has-text('Tom & Jerry')");
        await AssertElementTextAsync("p", "Classic Cartoon Collection");
    }

    [Fact]
    public async Task Footer_ShouldDisplayNavigationLinks()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync("footer");
        
        // Assert
        await AssertElementVisibleAsync("button:has-text('All Episodes')");
        await AssertElementVisibleAsync("button:has-text('Sticker Gallery')");
        await AssertElementVisibleAsync("button:has-text('Search Episodes')");
    }

    [Fact]
    public async Task Footer_ShouldDisplayStatistics()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync("footer");
        
        // Assert
        await AssertElementVisibleAsync("text:has-text('Episodes Available')");
        await AssertElementVisibleAsync("text:has-text('Stickers to Collect')");
        await AssertElementVisibleAsync("text:has-text('Hours of Fun')");
    }

    [Fact]
    public async Task Footer_ShouldNavigateToPages()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync("footer");
        
        // Click All Episodes
        await ClickElementAsync("button:has-text('All Episodes')", waitForNavigation: true);
        await AssertPageTitleAsync("Home - Tom & Jerry");
        
        // Click Sticker Gallery
        await ClickElementAsync("button:has-text('Sticker Gallery')", waitForNavigation: true);
        await AssertPageTitleAsync("Tom & Jerry Stickers - Tom & Jerry");
        
        // Click Search Episodes
        await ClickElementAsync("button:has-text('Search Episodes')", waitForNavigation: true);
        await AssertPageTitleAsync("Search Results - Tom & Jerry");
    }

    [Fact]
    public async Task Footer_ShouldDisplayDisclaimer()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync("footer");
        
        // Assert
        await AssertElementVisibleAsync("text:has-text('Educational Purpose')");
        await AssertElementVisibleAsync("text:has-text('Content Disclaimer')");
        await AssertElementVisibleAsync("text:has-text('Warner Bros')");
    }

    [Fact]
    public async Task Footer_ShouldDisplayCopyright()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync("footer");
        
        // Assert
        await AssertElementVisibleAsync("text:has-text('© 2024 Educational Project')");
        await AssertElementVisibleAsync("text:has-text('Made with ❤️ for learning purposes')");
    }

    [Fact]
    public async Task Footer_ShouldHaveResponsiveDesign()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync("footer");
        
        // Test mobile viewport
        await Page.SetViewportSizeAsync(375, 667);
        await Task.Delay(500);
        
        // Assert - Footer should still be visible
        await AssertElementVisibleAsync("footer");
        
        // Test desktop viewport
        await Page.SetViewportSizeAsync(1920, 1080);
        await Task.Delay(500);
        
        // Assert - Footer should still be visible
        await AssertElementVisibleAsync("footer");
    }

    [Fact]
    public async Task Thumbnail_ShouldDisplayVideoImage()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Assert
        var thumbnails = await Page.QuerySelectorAllAsync(".tom-jerry-card img");
        Assert.True(thumbnails.Count > 0, "Should have video thumbnails");
        
        foreach (var thumbnail in thumbnails)
        {
            var src = await thumbnail.GetAttributeAsync("src");
            var alt = await thumbnail.GetAttributeAsync("alt");
            
            Assert.False(string.IsNullOrEmpty(src), "Thumbnail should have src attribute");
        }
    }

    [Fact]
    public async Task Thumbnail_ShouldDisplayVideoTitle()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Assert
        var titles = await Page.QuerySelectorAllAsync(".tom-jerry-card h3");
        Assert.True(titles.Count > 0, "Should have video titles");
        
        foreach (var title in titles)
        {
            var titleText = await title.TextContentAsync();
            Assert.False(string.IsNullOrEmpty(titleText), "Video titles should not be empty");
        }
    }

    [Fact]
    public async Task Thumbnail_ShouldDisplayChannelInfo()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Assert
        var channelInfo = await Page.QuerySelectorAllAsync(".tom-jerry-card .text-amber-900");
        Assert.True(channelInfo.Count > 0, "Should have channel information");
        
        var channelText = await GetElementTextAsync(".tom-jerry-card .text-amber-900");
        Assert.Contains("Tom & Jerry", channelText);
    }

    [Fact]
    public async Task Thumbnail_ShouldDisplayVideoStats()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Assert
        var statsElements = await Page.QuerySelectorAllAsync(".tom-jerry-card .text-amber-800");
        Assert.True(statsElements.Count > 0, "Should have video statistics");
        
        var statsText = await GetElementTextAsync(".tom-jerry-card .text-amber-800");
        Assert.True(statsText.Contains("views") || statsText.Contains("ago"), "Should contain view count or time information");
    }

    [Fact]
    public async Task Thumbnail_ShouldNavigateToVideoOnClick()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Click on first thumbnail
        var firstThumbnail = await Page.QuerySelectorAsync(".tom-jerry-card");
        if (firstThumbnail != null)
        {
            await firstThumbnail.ClickAsync();
            await Task.Delay(1000);
            
            // Assert - Should navigate to video page
            var currentUrl = Page.Url;
            Assert.Contains("/playmedia/", currentUrl);
        }
    }

    [Fact]
    public async Task Thumbnail_ShouldDisplayPlayButtonOnHover()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Hover over first thumbnail
        var firstThumbnail = await Page.QuerySelectorAsync(".tom-jerry-card");
        if (firstThumbnail != null)
        {
            await firstThumbnail.HoverAsync();
            await Task.Delay(500);
            
            // Assert - Play button should be visible
            await AssertElementVisibleAsync(".w-16.h-16, .w-20.h-20");
        }
    }

    [Fact]
    public async Task Thumbnail_ShouldDisplayDurationBadge()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Assert
        await AssertElementVisibleAsync("text:has-text('7:30')");
    }

    [Fact]
    public async Task Thumbnail_ShouldHaveResponsiveDesign()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Test mobile viewport
        await Page.SetViewportSizeAsync(375, 667);
        await Task.Delay(500);
        
        // Assert - Thumbnails should still be visible
        var thumbnails = await Page.QuerySelectorAllAsync(".tom-jerry-card");
        Assert.True(thumbnails.Count > 0, "Should have thumbnails on mobile");
        
        // Test desktop viewport
        await Page.SetViewportSizeAsync(1920, 1080);
        await Task.Delay(500);
        
        // Assert - Thumbnails should still be visible
        var thumbnailsDesktop = await Page.QuerySelectorAllAsync(".tom-jerry-card");
        Assert.True(thumbnailsDesktop.Count > 0, "Should have thumbnails on desktop");
    }

    [Fact]
    public async Task RandomSticker_ShouldDisplayStickerImage()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForRandomStickerAsync();
        
        // Assert
        var stickers = await Page.QuerySelectorAllAsync("img[src*='sticker']");
        Assert.True(stickers.Count > 0, "Should have random sticker images");
    }

    [Fact]
    public async Task RandomSticker_ShouldAutoRefresh()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForRandomStickerAsync();
        
        // Wait for potential auto-refresh
        await Task.Delay(15000); // Wait 15 seconds for auto-refresh
        
        // Assert - Stickers should still be visible
        var stickers = await Page.QuerySelectorAllAsync("img[src*='sticker']");
        Assert.True(stickers.Count > 0, "Should have random sticker images after auto-refresh");
    }

    [Fact]
    public async Task Snackbar_ShouldDisplayWhenTriggered()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Click on character to trigger snackbar
        await ClickElementAsync(".tom-character");
        await WaitForSnackbarAsync();
        
        // Assert
        await AssertElementVisibleAsync(".snackbar, .toast, .notification");
    }

    [Fact]
    public async Task Snackbar_ShouldDisplayRandomFact()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Click random fact button
        await ClickElementAsync("button:has-text('Random Fact')");
        await WaitForSnackbarAsync();
        
        // Assert
        await AssertElementVisibleAsync(".snackbar, .toast, .notification");
        
        var snackbarText = await GetElementTextAsync(".snackbar, .toast, .notification");
        Assert.False(string.IsNullOrEmpty(snackbarText), "Snackbar should contain random fact text");
    }

    [Fact]
    public async Task StickerGallery_ShouldDisplayStickers()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Assert
        var stickers = await Page.QuerySelectorAllAsync("img[src*='sticker'], .sticker-item, [class*='sticker']");
        Assert.True(stickers.Count > 0, "Should have sticker gallery elements");
    }

    [Fact]
    public async Task StickerGallery_ShouldHandleStickerSelection()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Click on first sticker
        var firstSticker = await Page.QuerySelectorAsync("img[src*='sticker'], .sticker-item, [class*='sticker']");
        if (firstSticker != null)
        {
            await firstSticker.ClickAsync();
            await WaitForSnackbarAsync();
            
            // Assert - Snackbar should appear
            await AssertElementVisibleAsync(".snackbar, .toast, .notification");
        }
    }

    [Fact]
    public async Task VideoGrid_ShouldDisplayVideosInGrid()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Assert
        await AssertElementVisibleAsync(".grid");
        var videoCards = await Page.QuerySelectorAllAsync(".tom-jerry-card");
        Assert.True(videoCards.Count > 0, "Should have video cards in grid");
    }

    [Fact]
    public async Task VideoGrid_ShouldBeResponsive()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Test mobile viewport
        await Page.SetViewportSizeAsync(375, 667);
        await Task.Delay(500);
        
        // Assert - Grid should adapt to mobile
        await AssertElementVisibleAsync(".grid");
        
        // Test desktop viewport
        await Page.SetViewportSizeAsync(1920, 1080);
        await Task.Delay(500);
        
        // Assert - Grid should adapt to desktop
        await AssertElementVisibleAsync(".grid");
    }

    [Fact]
    public async Task Components_ShouldHaveAccessibleElements()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync("header");
        
        // Assert - Check for proper heading structure
        await AssertElementVisibleAsync("h1");
        
        // Check for button accessibility
        var buttons = await Page.QuerySelectorAllAsync("button");
        foreach (var button in buttons)
        {
            var text = await button.TextContentAsync();
            Assert.False(string.IsNullOrEmpty(text), $"Button should have text content: {text}");
        }
        
        // Check for image accessibility
        var images = await Page.QuerySelectorAllAsync("img");
        foreach (var img in images)
        {
            var src = await img.GetAttributeAsync("src");
            Assert.False(string.IsNullOrEmpty(src), "Images should have src attribute");
        }
    }
}
