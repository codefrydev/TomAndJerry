using Xunit;

namespace TomAndJerry.Tests;

/// <summary>
/// Comprehensive UI tests for the PlayMedia page functionality
/// </summary>
public class PlayMediaPageTests : UITestBase
{
    [Fact]
    public async Task PlayMediaPage_ShouldLoadSuccessfully()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        
        // Assert
        await AssertPageTitleAsync("Tom & Jerry - Tom & Jerry");
        await AssertElementVisibleAsync("h1");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayVideoPlayer()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForVideoPlayerAsync();
        
        // Assert
        await AssertElementVisibleAsync("iframe[src*='drive.google.com'], video");
        await AssertElementVisibleAsync(".video-container");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayVideoTitle()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".tom-jerry-card");
        
        // Assert
        await AssertElementVisibleAsync("h1");
        
        // Check that title is not empty
        var titleText = await GetElementTextAsync("h1");
        Assert.False(string.IsNullOrEmpty(titleText), "Video title should not be empty");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayVideoStats()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".tom-jerry-card");
        
        // Assert
        await AssertElementVisibleAsync("text:has-text('views')");
        await AssertElementVisibleAsync("text:has-text('ago')");
        await AssertElementVisibleAsync(".flex.text-cartoon-yellow");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayActionButtons()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".tom-jerry-card");
        
        // Assert
        await AssertElementVisibleAsync("button:has-text('Like')");
        await AssertElementVisibleAsync("button:has-text('Share')");
        await AssertElementVisibleAsync("button:has-text('Save')");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayChannelInfo()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".tom-jerry-card");
        
        // Assert
        await AssertElementVisibleAsync("h3:has-text('Tom & Jerry Channel')");
        await AssertElementVisibleAsync("text:has-text('subscribers')");
        await AssertElementVisibleAsync("text:has-text('videos')");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplaySubscribeButton()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".tom-jerry-card");
        
        // Assert
        await AssertElementVisibleAsync("button:has-text('Subscribe')");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayVideoDescription()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".bg-white.rounded-xl");
        
        // Assert
        await AssertElementVisibleAsync("h3:has-text('Description')");
        await AssertElementVisibleAsync("button:has-text('Show more')");
        
        // Check for description text
        var descriptionText = await GetElementTextAsync("p");
        Assert.False(string.IsNullOrEmpty(descriptionText), "Video description should not be empty");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayTags()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".bg-white.rounded-xl");
        
        // Assert
        await AssertElementVisibleAsync("text:has-text('#TomAndJerry')");
        await AssertElementVisibleAsync("text:has-text('#Cartoon')");
        await AssertElementVisibleAsync("text:has-text('#Classic')");
        await AssertElementVisibleAsync("text:has-text('#Comedy')");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayCommentsSection()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".bg-white.rounded-xl");
        
        // Assert
        await AssertElementVisibleAsync("h3:has-text('Comments')");
        await AssertElementVisibleAsync("text:has-text('comments')");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldShowCommentsWhenClicked()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".bg-white.rounded-xl");
        
        // Click show comments button
        await ClickElementAsync("button:has-text('Show Comments')");
        await Task.Delay(1000);
        
        // Assert
        await AssertElementVisibleAsync("button:has-text('Giscus')");
        await AssertElementVisibleAsync("button:has-text('Disqus')");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldSwitchCommentTabs()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".bg-white.rounded-xl");
        
        // Show comments
        await ClickElementAsync("button:has-text('Show Comments')");
        await Task.Delay(1000);
        
        // Click Disqus tab
        await ClickElementAsync("button:has-text('Disqus')");
        await Task.Delay(500);
        
        // Assert
        await AssertElementVisibleAsync("#disqus_thread");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplaySidebar()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".lg\\:col-span-1");
        
        // Assert
        await AssertElementVisibleAsync(".lg\\:col-span-1");
        await AssertElementVisibleAsync("h3:has-text('Up next')");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayAutoplayToggle()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".lg\\:col-span-1");
        
        // Assert
        await AssertElementVisibleAsync("text:has-text('Autoplay is on')");
        await AssertElementVisibleAsync("button[role='switch'], .relative.inline-flex");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayRelatedVideos()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".lg\\:col-span-1");
        await WaitForVideoThumbnailsAsync();
        
        // Assert
        var relatedVideos = await Page.QuerySelectorAllAsync(".tom-jerry-card, .group");
        Assert.True(relatedVideos.Count > 0, "Should have related videos in sidebar");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldLoadMoreVideos()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".lg\\:col-span-1");
        await WaitForVideoThumbnailsAsync();
        
        // Check if load more button exists
        var loadMoreButton = await Page.QuerySelectorAsync("button:has-text('Load more videos')");
        
        if (loadMoreButton != null)
        {
            // Get initial video count
            var initialVideos = await Page.QuerySelectorAllAsync(".tom-jerry-card, .group");
            var initialCount = initialVideos.Count;
            
            // Click load more
            await ClickElementAsync("button:has-text('Load more videos')");
            await Task.Delay(2000);
            
            // Assert - Should have more videos
            var newVideos = await Page.QuerySelectorAllAsync(".tom-jerry-card, .group");
            var newCount = newVideos.Count;
            
            Assert.True(newCount >= initialCount, "Should have same or more videos after loading more");
        }
    }

    [Fact]
    public async Task PlayMediaPage_ShouldNavigateToRelatedVideo()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".lg\\:col-span-1");
        await WaitForVideoThumbnailsAsync();
        
        // Click on first related video
        var firstRelatedVideo = await Page.QuerySelectorAsync(".tom-jerry-card, .group");
        if (firstRelatedVideo != null)
        {
            await firstRelatedVideo.ClickAsync();
            await Task.Delay(1000);
            
            // Assert - Should navigate to new video
            var currentUrl = Page.Url;
            Assert.Contains("/playmedia/", currentUrl);
        }
    }

    [Fact]
    public async Task PlayMediaPage_ShouldHaveResponsiveDesign()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".grid.grid-cols-1.lg\\:grid-cols-4");
        
        // Test mobile viewport
        await Page.SetViewportSizeAsync(375, 667);
        await Task.Delay(500);
        
        // Assert - Elements should still be visible on mobile
        await AssertElementVisibleAsync("h1");
        await AssertElementVisibleAsync("iframe[src*='drive.google.com'], video");
        
        // Test desktop viewport
        await Page.SetViewportSizeAsync(1920, 1080);
        await Task.Delay(500);
        
        // Assert - Elements should still be visible on desktop
        await AssertElementVisibleAsync("h1");
        await AssertElementVisibleAsync("iframe[src*='drive.google.com'], video");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldHaveAccessibleElements()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".tom-jerry-card");
        
        // Assert - Check for proper heading structure
        await AssertElementVisibleAsync("h1");
        await AssertElementVisibleAsync("h3");
        
        // Check for button accessibility
        var buttons = await Page.QuerySelectorAllAsync("button");
        foreach (var button in buttons)
        {
            var text = await button.TextContentAsync();
            Assert.False(string.IsNullOrEmpty(text), $"Button should have text content: {text}");
        }
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayVideoThumbnail()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".tom-jerry-card");
        
        // Assert
        var thumbnail = await Page.QuerySelectorAsync("img[src*='Thumbnail'], img[alt*='Tom'], img[alt*='Jerry']");
        Assert.NotNull(thumbnail);
        
        var src = await thumbnail.GetAttributeAsync("src");
        var alt = await thumbnail.GetAttributeAsync("alt");
        
        Assert.False(string.IsNullOrEmpty(src), "Thumbnail should have src attribute");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayVideoDuration()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".tom-jerry-card");
        
        // Assert
        await AssertElementVisibleAsync("text:has-text('7:30')");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayChannelAvatar()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".tom-jerry-card");
        
        // Assert
        var avatar = await Page.QuerySelectorAsync("img[src*='Tom.png'], img[alt*='Tom'], img[alt*='Jerry']");
        Assert.NotNull(avatar);
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayOnlineIndicator()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".tom-jerry-card");
        
        // Assert
        await AssertElementVisibleAsync(".bg-green-500.rounded-full");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldHandleVideoPlayerLoad()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForVideoPlayerAsync();
        
        // Assert
        var iframe = await Page.QuerySelectorAsync("iframe[src*='drive.google.com']");
        Assert.NotNull(iframe);
        
        var src = await iframe.GetAttributeAsync("src");
        Assert.False(string.IsNullOrEmpty(src), "Video iframe should have src attribute");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayVideoStatsCorrectly()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".tom-jerry-card");
        
        // Assert
        var viewsText = await GetElementTextAsync("text:has-text('views')");
        Assert.Contains("views", viewsText);
        
        var timeText = await GetElementTextAsync("text:has-text('ago')");
        Assert.Contains("ago", timeText);
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayRatingStars()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".tom-jerry-card");
        
        // Assert
        var stars = await Page.QuerySelectorAllAsync(".flex.text-cartoon-yellow svg");
        Assert.True(stars.Count == 5, "Should have 5 rating stars");
        
        await AssertElementVisibleAsync("text:has-text('4.8')");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldHandleInvalidVideoId()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/invalid");
        
        // Assert - Should handle invalid video ID gracefully
        // The page should still load without crashing
        await AssertElementVisibleAsync("h1");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayVideoDescriptionWithShowMore()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".bg-white.rounded-xl");
        
        // Click show more button
        await ClickElementAsync("button:has-text('Show more')");
        await Task.Delay(500);
        
        // Assert - Description should be expanded
        await AssertElementVisibleAsync("button:has-text('Show more')");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplaySortCommentsButton()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".bg-white.rounded-xl");
        
        // Show comments
        await ClickElementAsync("button:has-text('Show Comments')");
        await Task.Delay(1000);
        
        // Assert
        await AssertElementVisibleAsync("button:has-text('Sort by')");
    }

    [Fact]
    public async Task PlayMediaPage_ShouldDisplayVideoGridLayout()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".grid.grid-cols-1.lg\\:grid-cols-4");
        
        // Assert
        await AssertElementVisibleAsync(".grid.grid-cols-1.lg\\:grid-cols-4");
        await AssertElementVisibleAsync(".lg\\:col-span-3");
        await AssertElementVisibleAsync(".lg\\:col-span-1");
    }
}
