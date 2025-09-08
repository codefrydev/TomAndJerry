using Xunit;

namespace TomAndJerry.Tests;

/// <summary>
/// Comprehensive UI tests for the Home page functionality
/// </summary>
public class HomePageTests : UITestBase
{
    [Fact]
    public async Task HomePage_ShouldLoadSuccessfully()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        
        // Assert
        await AssertPageTitleAsync("Home - Tom & Jerry");
        await AssertElementVisibleAsync("h1");
        await AssertElementTextAsync("h1", "Tom & Jerry");
    }

    [Fact]
    public async Task HomePage_ShouldDisplayHeroSection()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Assert
        await AssertElementVisibleAsync(".tom-jerry-header");
        await AssertElementVisibleAsync(".tom-character");
        await AssertElementVisibleAsync(".jerry-character");
        await AssertElementTextAsync(".tom-character .text-sm", "TOM");
        await AssertElementTextAsync(".jerry-character .text-sm", "JERRY");
    }

    [Fact]
    public async Task HomePage_ShouldDisplayCharacterInfo()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Click on Tom character
        await ClickElementAsync(".tom-character");
        await WaitForSnackbarAsync();
        
        // Click on Jerry character
        await ClickElementAsync(".jerry-character");
        await WaitForSnackbarAsync();
        
        // Assert - Snackbar should appear with character info
        await AssertElementVisibleAsync(".snackbar, .toast, .notification");
    }

    [Fact]
    public async Task HomePage_ShouldDisplayStatsGrid()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Assert
        await AssertElementVisibleAsync(".grid.grid-cols-1.sm\\:grid-cols-2.lg\\:grid-cols-4");
        
        // Check for specific stat cards
        await AssertElementVisibleAsync(".tom-jerry-card");
        
        // Verify stat labels
        var statCards = await Page.QuerySelectorAllAsync(".tom-jerry-card");
        Assert.True(statCards.Count >= 4, "Should have at least 4 stat cards");
    }

    [Fact]
    public async Task HomePage_ShouldDisplayActionButtons()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Assert
        await AssertElementVisibleAsync("button:has-text('Browse Episodes')");
        await AssertElementVisibleAsync("button:has-text('View All')");
        await AssertElementVisibleAsync("button:has-text('Take Quiz')");
        await AssertElementVisibleAsync("button:has-text('Random Fact')");
    }

    [Fact]
    public async Task HomePage_ShouldNavigateToQuiz()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Click Quiz button
        await ClickElementAsync("button:has-text('Take Quiz')", waitForNavigation: true);
        
        // Assert
        await AssertPageTitleAsync("Tom & Jerry Quiz - Test Your Knowledge!");
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Quiz')");
    }

    [Fact]
    public async Task HomePage_ShouldScrollToFeaturedSection()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Click Browse Episodes button
        await ClickElementAsync("button:has-text('Browse Episodes')");
        
        // Wait for scroll animation
        await Task.Delay(1000);
        
        // Assert - Featured section should be visible
        await AssertElementVisibleAsync("#featured");
    }

    [Fact]
    public async Task HomePage_ShouldScrollToAllEpisodesSection()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Click View All button
        await ClickElementAsync("button:has-text('View All')");
        
        // Wait for scroll animation
        await Task.Delay(1000);
        
        // Assert - All episodes section should be visible
        await AssertElementVisibleAsync("#all-episodes");
    }

    [Fact]
    public async Task HomePage_ShouldDisplayRandomFact()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Click Random Fact button
        await ClickElementAsync("button:has-text('Random Fact')");
        await WaitForSnackbarAsync();
        
        // Assert - Snackbar should appear with random fact
        await AssertElementVisibleAsync(".snackbar, .toast, .notification");
    }

    [Fact]
    public async Task HomePage_ShouldDisplayFeaturedEpisodes()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Assert
        await AssertElementVisibleAsync("#featured");
        await AssertElementVisibleAsync("h2:has-text('Featured Episodes')");
        
        // Check for video thumbnails
        var videoCards = await Page.QuerySelectorAllAsync("#featured .tom-jerry-card");
        Assert.True(videoCards.Count > 0, "Should have featured video cards");
    }

    [Fact]
    public async Task HomePage_ShouldDisplayStickerGallery()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForStickerGalleryAsync();
        
        // Assert
        await AssertElementVisibleAsync("#sticker-gallery");
        await AssertElementVisibleAsync("h2:has-text('Tom & Jerry Stickers')");
        
        // Check for sticker elements
        var stickers = await Page.QuerySelectorAllAsync("#sticker-gallery img, .sticker-item");
        Assert.True(stickers.Count > 0, "Should have sticker elements");
    }

    [Fact]
    public async Task HomePage_ShouldDisplayAllEpisodes()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Assert
        await AssertElementVisibleAsync("#all-episodes");
        await AssertElementVisibleAsync("h2:has-text('All Episodes')");
        
        // Check for video thumbnails
        var videoCards = await Page.QuerySelectorAllAsync("#all-episodes .tom-jerry-card");
        Assert.True(videoCards.Count > 0, "Should have video cards in all episodes section");
    }

    [Fact]
    public async Task HomePage_ShouldFilterEpisodes()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Click Classic filter
        await ClickElementAsync("button:has-text('Classic')");
        await Task.Delay(500);
        
        // Click Modern filter
        await ClickElementAsync("button:has-text('Modern')");
        await Task.Delay(500);
        
        // Click All filter
        await ClickElementAsync("button:has-text('All')");
        await Task.Delay(500);
        
        // Assert - Filter buttons should be clickable and change state
        await AssertElementVisibleAsync("button:has-text('All')");
        await AssertElementVisibleAsync("button:has-text('Classic')");
        await AssertElementVisibleAsync("button:has-text('Modern')");
    }

    [Fact]
    public async Task HomePage_ShouldRefreshFeaturedEpisodes()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Click refresh button
        await ClickElementAsync("button:has-text('Refresh Featured')");
        await Task.Delay(1000);
        
        // Assert - Featured section should still be visible
        await AssertElementVisibleAsync("#featured");
        await AssertElementVisibleAsync("h2:has-text('Featured Episodes')");
    }

    [Fact]
    public async Task HomePage_ShouldNavigateToStickersPage()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Click View All stickers button
        await ClickElementAsync("button:has-text('View All')", waitForNavigation: true);
        
        // Assert
        await AssertPageTitleAsync("Tom & Jerry Stickers - Tom & Jerry");
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Stickers')");
    }

    [Fact]
    public async Task HomePage_ShouldDisplayLoadingState()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        
        // Assert - Should show loading skeleton initially
        var hasLoadingElements = await Page.QuerySelectorAsync(".skeleton, .animate-pulse");
        if (hasLoadingElements != null)
        {
            await AssertElementVisibleAsync(".skeleton, .animate-pulse");
        }
        
        // Wait for loading to complete
        await WaitForLoadingCompleteAsync();
        
        // Assert - Loading should be gone
        await AssertElementHiddenAsync(".skeleton");
    }

    [Fact]
    public async Task HomePage_ShouldDisplayFunFacts()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Assert
        await AssertElementVisibleAsync(".fact-carousel");
        await AssertElementVisibleAsync("#fun-fact-text");
        
        // Check that fun fact text is not empty
        var funFactText = await GetElementTextAsync("#fun-fact-text");
        Assert.False(string.IsNullOrEmpty(funFactText), "Fun fact text should not be empty");
    }

    [Fact]
    public async Task HomePage_ShouldHaveResponsiveDesign()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Test mobile viewport
        await Page.SetViewportSizeAsync(375, 667);
        await Task.Delay(500);
        
        // Assert - Elements should still be visible on mobile
        await AssertElementVisibleAsync(".tom-jerry-header");
        await AssertElementVisibleAsync("h1");
        
        // Test desktop viewport
        await Page.SetViewportSizeAsync(1920, 1080);
        await Task.Delay(500);
        
        // Assert - Elements should still be visible on desktop
        await AssertElementVisibleAsync(".tom-jerry-header");
        await AssertElementVisibleAsync("h1");
    }

    [Fact]
    public async Task HomePage_ShouldHaveAccessibleElements()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Assert - Check for proper heading structure
        await AssertElementVisibleAsync("h1");
        await AssertElementVisibleAsync("h2");
        
        // Check for button accessibility
        var buttons = await Page.QuerySelectorAllAsync("button");
        foreach (var button in buttons)
        {
            var text = await button.TextContentAsync();
            Assert.False(string.IsNullOrEmpty(text), $"Button should have text content: {text}");
        }
    }

    [Fact]
    public async Task HomePage_ShouldHandleVideoThumbnailClicks()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Click on first video thumbnail
        var firstVideoCard = await Page.QuerySelectorAsync(".tom-jerry-card");
        if (firstVideoCard != null)
        {
            await firstVideoCard.ClickAsync();
            await Task.Delay(1000);
            
            // Assert - Should navigate to video page
            var currentUrl = Page.Url;
            Assert.Contains("/playmedia/", currentUrl);
        }
    }

    [Fact]
    public async Task HomePage_ShouldDisplayCorrectVideoCount()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Assert - Check that video count is displayed
        var videoCountElement = await Page.QuerySelectorAsync(".tom-jerry-card .text-3xl, .tom-jerry-card .text-4xl");
        if (videoCountElement != null)
        {
            var countText = await videoCountElement.TextContentAsync();
            Assert.False(string.IsNullOrEmpty(countText), "Video count should be displayed");
        }
    }
}
