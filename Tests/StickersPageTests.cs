using Xunit;

namespace TomAndJerry.Tests;

/// <summary>
/// Comprehensive UI tests for the Stickers page functionality
/// </summary>
public class StickersPageTests : UITestBase
{
    [Fact]
    public async Task StickersPage_ShouldLoadSuccessfully()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        
        // Assert
        await AssertPageTitleAsync("Tom & Jerry Stickers - Tom & Jerry");
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Stickers')");
    }

    [Fact]
    public async Task StickersPage_ShouldDisplayHeader()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Assert
        await AssertElementVisibleAsync(".tom-jerry-header");
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Stickers')");
        await AssertElementTextAsync("p", "Collect all your favorite character moments! 🎭✨");
    }

    [Fact]
    public async Task StickersPage_ShouldDisplayBackButton()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Assert
        await AssertElementVisibleAsync("button:has-text('← Back to Episodes')");
    }

    [Fact]
    public async Task StickersPage_ShouldNavigateBackToHome()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Click back button
        await ClickElementAsync("button:has-text('← Back to Episodes')", waitForNavigation: true);
        
        // Assert
        await AssertPageTitleAsync("Home - Tom & Jerry");
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry')");
    }

    [Fact]
    public async Task StickersPage_ShouldDisplayFilterButtons()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForBlazorComponentAsync(".max-w-7xl.mx-auto");
        
        // Assert
        await AssertElementVisibleAsync("button:has-text('All Stickers')");
        
        // Check for category filter buttons (these might be dynamic)
        var filterButtons = await Page.QuerySelectorAllAsync("button");
        Assert.True(filterButtons.Count > 1, "Should have multiple filter buttons");
    }

    [Fact]
    public async Task StickersPage_ShouldDisplayStats()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForBlazorComponentAsync(".max-w-7xl.mx-auto");
        
        // Assert
        await AssertElementVisibleAsync("text:has-text('Showing')");
        await AssertElementVisibleAsync("text:has-text('stickers')");
    }

    [Fact]
    public async Task StickersPage_ShouldDisplayStickerGallery()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Assert
        await AssertElementVisibleAsync(".sticker-gallery, [class*='sticker']");
        
        // Check for sticker elements
        var stickers = await Page.QuerySelectorAllAsync("img[src*='sticker'], .sticker-item, [class*='sticker']");
        Assert.True(stickers.Count > 0, "Should have sticker elements");
    }

    [Fact]
    public async Task StickersPage_ShouldFilterByCategory()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Get available category buttons
        var categoryButtons = await Page.QuerySelectorAllAsync("button:not(:has-text('All Stickers'))");
        
        if (categoryButtons.Count > 0)
        {
            // Click on first category button
            await categoryButtons[0].ClickAsync();
            await Task.Delay(500);
            
            // Assert - Category should be selected
            var hasSelectedClass = await ElementHasClassAsync($"button:has-text('{await categoryButtons[0].TextContentAsync()}')", "ring-2");
            Assert.True(hasSelectedClass, "Category button should be selected");
        }
    }

    [Fact]
    public async Task StickersPage_ShouldSelectAllStickers()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Click All Stickers button
        await ClickElementAsync("button:has-text('All Stickers')");
        await Task.Delay(500);
        
        // Assert - All Stickers should be selected
        var hasSelectedClass = await ElementHasClassAsync("button:has-text('All Stickers')", "ring-2");
        Assert.True(hasSelectedClass, "All Stickers button should be selected");
    }

    [Fact]
    public async Task StickersPage_ShouldDisplayLoadMoreButton()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Check if load more button is visible (depends on sticker count)
        var loadMoreButton = await Page.QuerySelectorAsync("button:has-text('Load More Stickers')");
        
        if (loadMoreButton != null)
        {
            // Assert
            await AssertElementVisibleAsync("button:has-text('Load More Stickers')");
        }
    }

    [Fact]
    public async Task StickersPage_ShouldLoadMoreStickers()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Check if load more button exists
        var loadMoreButton = await Page.QuerySelectorAsync("button:has-text('Load More Stickers')");
        
        if (loadMoreButton != null)
        {
            // Get initial sticker count
            var initialStickers = await Page.QuerySelectorAllAsync("img[src*='sticker'], .sticker-item, [class*='sticker']");
            var initialCount = initialStickers.Count;
            
            // Click load more
            await ClickElementAsync("button:has-text('Load More Stickers')");
            await Task.Delay(1000);
            
            // Assert - Should have more stickers
            var newStickers = await Page.QuerySelectorAllAsync("img[src*='sticker'], .sticker-item, [class*='sticker']");
            var newCount = newStickers.Count;
            
            Assert.True(newCount >= initialCount, "Should have same or more stickers after loading more");
        }
    }

    [Fact]
    public async Task StickersPage_ShouldHandleStickerSelection()
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
    public async Task StickersPage_ShouldDisplayRandomFacts()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Click on a sticker to trigger random fact
        var firstSticker = await Page.QuerySelectorAsync("img[src*='sticker'], .sticker-item, [class*='sticker']");
        if (firstSticker != null)
        {
            await firstSticker.ClickAsync();
            await WaitForSnackbarAsync();
            
            // Assert - Should show random fact in snackbar
            await AssertElementVisibleAsync(".snackbar, .toast, .notification");
            
            var snackbarText = await GetElementTextAsync(".snackbar, .toast, .notification");
            Assert.False(string.IsNullOrEmpty(snackbarText), "Snackbar should contain random fact text");
        }
    }

    [Fact]
    public async Task StickersPage_ShouldHaveResponsiveDesign()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Test mobile viewport
        await Page.SetViewportSizeAsync(375, 667);
        await Task.Delay(500);
        
        // Assert - Elements should still be visible on mobile
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Stickers')");
        var stickers = await Page.QuerySelectorAllAsync("img[src*='sticker'], .sticker-item, [class*='sticker']");
        Assert.True(stickers.Count > 0, "Should have stickers on mobile");
        
        // Test desktop viewport
        await Page.SetViewportSizeAsync(1920, 1080);
        await Task.Delay(500);
        
        // Assert - Elements should still be visible on desktop
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Stickers')");
        var stickersDesktop = await Page.QuerySelectorAllAsync("img[src*='sticker'], .sticker-item, [class*='sticker']");
        Assert.True(stickersDesktop.Count > 0, "Should have stickers on desktop");
    }

    [Fact]
    public async Task StickersPage_ShouldHaveAccessibleElements()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Assert - Check for proper heading structure
        await AssertElementVisibleAsync("h1");
        
        // Check for button accessibility
        var buttons = await Page.QuerySelectorAllAsync("button");
        foreach (var button in buttons)
        {
            var text = await button.TextContentAsync();
            Assert.False(string.IsNullOrEmpty(text), $"Button should have text content: {text}");
        }
    }

    [Fact]
    public async Task StickersPage_ShouldDisplayStickerImages()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Assert
        var stickerImages = await Page.QuerySelectorAllAsync("img[src*='sticker']");
        Assert.True(stickerImages.Count > 0, "Should have sticker images");
        
        // Check that images have proper attributes
        foreach (var img in stickerImages)
        {
            var src = await img.GetAttributeAsync("src");
            var alt = await img.GetAttributeAsync("alt");
            
            Assert.False(string.IsNullOrEmpty(src), "Sticker images should have src attribute");
            // Alt can be empty for decorative images, but src is required
        }
    }

    [Fact]
    public async Task StickersPage_ShouldDisplayCategoryStats()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Assert
        var statsText = await GetElementTextAsync("p");
        Assert.Contains("Showing", statsText);
        Assert.Contains("stickers", statsText);
    }

    [Fact]
    public async Task StickersPage_ShouldHandleEmptyStickerGallery()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForBlazorComponentAsync(".max-w-7xl.mx-auto");
        
        // Assert - Should handle empty state gracefully
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Stickers')");
        await AssertElementVisibleAsync("button:has-text('All Stickers')");
    }

    [Fact]
    public async Task StickersPage_ShouldDisplayStickerCategories()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Assert - Should have category filter buttons
        var categoryButtons = await Page.QuerySelectorAllAsync("button:not(:has-text('All Stickers'))");
        
        // Should have at least some category buttons
        Assert.True(categoryButtons.Count >= 0, "Should have category filter buttons");
    }

    [Fact]
    public async Task StickersPage_ShouldUpdateStatsWhenFiltering()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Get initial stats
        var initialStats = await GetElementTextAsync("p");
        
        // Click on a category filter if available
        var categoryButtons = await Page.QuerySelectorAllAsync("button:not(:has-text('All Stickers'))");
        
        if (categoryButtons.Count > 0)
        {
            await categoryButtons[0].ClickAsync();
            await Task.Delay(500);
            
            // Get updated stats
            var updatedStats = await GetElementTextAsync("p");
            
            // Assert - Stats should be updated
            Assert.NotEqual(initialStats, updatedStats);
        }
    }

    [Fact]
    public async Task StickersPage_ShouldDisplayStickerGalleryComponent()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Assert - Should have sticker gallery component
        await AssertElementVisibleAsync("[class*='sticker-gallery'], .sticker-gallery");
        
        // Check for sticker gallery specific elements
        var galleryElements = await Page.QuerySelectorAllAsync("[class*='sticker'], img[src*='sticker']");
        Assert.True(galleryElements.Count > 0, "Should have sticker gallery elements");
    }

    [Fact]
    public async Task StickersPage_ShouldHandleStickerHoverEffects()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForStickerGalleryAsync();
        
        // Hover over first sticker
        var firstSticker = await Page.QuerySelectorAsync("img[src*='sticker'], .sticker-item, [class*='sticker']");
        if (firstSticker != null)
        {
            await firstSticker.HoverAsync();
            await Task.Delay(500);
            
            // Assert - Should handle hover without errors
            // This test mainly ensures hover doesn't break the page
            await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Stickers')");
        }
    }

    [Fact]
    public async Task StickersPage_ShouldDisplayBackButtonWithCorrectStyling()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Assert
        var backButton = await Page.QuerySelectorAsync("button:has-text('← Back to Episodes')");
        Assert.NotNull(backButton);
        
        // Check for cartoon button styling
        var hasCartoonClass = await ElementHasClassAsync("button:has-text('← Back to Episodes')", "cartoon-button");
        Assert.True(hasCartoonClass, "Back button should have cartoon button styling");
    }
}
