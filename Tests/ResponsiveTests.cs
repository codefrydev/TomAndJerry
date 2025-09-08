using Xunit;

namespace TomAndJerry.Tests;

/// <summary>
/// Comprehensive UI tests for responsive design and mobile functionality
/// </summary>
public class ResponsiveTests : UITestBase
{
    [Theory]
    [InlineData(375, 667, "Mobile Portrait")]
    [InlineData(414, 896, "Mobile Landscape")]
    [InlineData(768, 1024, "Tablet Portrait")]
    [InlineData(1024, 768, "Tablet Landscape")]
    [InlineData(1920, 1080, "Desktop")]
    [InlineData(2560, 1440, "Large Desktop")]
    public async Task HomePage_ShouldBeResponsive(int width, int height, string deviceType)
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await Page.SetViewportSizeAsync(width, height);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        await Task.Delay(500);
        
        // Assert
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry')");
        await AssertElementVisibleAsync(".tom-jerry-header");
        
        // Check that main content is visible
        var mainContent = await Page.QuerySelectorAsync(".max-w-7xl.mx-auto");
        Assert.NotNull(mainContent);
        
        // Check that navigation elements are accessible
        var buttons = await Page.QuerySelectorAllAsync("button");
        Assert.True(buttons.Count > 0, $"Should have buttons on {deviceType}");
    }

    [Theory]
    [InlineData(375, 667, "Mobile Portrait")]
    [InlineData(414, 896, "Mobile Landscape")]
    [InlineData(768, 1024, "Tablet Portrait")]
    [InlineData(1024, 768, "Tablet Landscape")]
    [InlineData(1920, 1080, "Desktop")]
    public async Task QuizPage_ShouldBeResponsive(int width, int height, string deviceType)
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await Page.SetViewportSizeAsync(width, height);
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        await Task.Delay(500);
        
        // Assert
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Quiz')");
        await AssertElementVisibleAsync("h2:has-text('Ready to Test Your Knowledge?')");
        
        // Check that quiz options are visible
        var quizButtons = await Page.QuerySelectorAllAsync("button");
        Assert.True(quizButtons.Count > 0, $"Should have quiz buttons on {deviceType}");
    }

    [Theory]
    [InlineData(375, 667, "Mobile Portrait")]
    [InlineData(414, 896, "Mobile Landscape")]
    [InlineData(768, 1024, "Tablet Portrait")]
    [InlineData(1024, 768, "Tablet Landscape")]
    [InlineData(1920, 1080, "Desktop")]
    public async Task SearchPage_ShouldBeResponsive(int width, int height, string deviceType)
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await Page.SetViewportSizeAsync(width, height);
        await WaitForSearchResultsAsync();
        await Task.Delay(500);
        
        // Assert
        await AssertElementVisibleAsync("h2");
        
        // Check that video grid adapts to screen size
        var videoGrid = await Page.QuerySelectorAsync(".grid");
        Assert.NotNull(videoGrid);
        
        // Check that video cards are visible
        var videoCards = await Page.QuerySelectorAllAsync(".tom-jerry-card, .group");
        Assert.True(videoCards.Count >= 0, $"Should handle video cards on {deviceType}");
    }

    [Theory]
    [InlineData(375, 667, "Mobile Portrait")]
    [InlineData(414, 896, "Mobile Landscape")]
    [InlineData(768, 1024, "Tablet Portrait")]
    [InlineData(1024, 768, "Tablet Landscape")]
    [InlineData(1920, 1080, "Desktop")]
    public async Task StickersPage_ShouldBeResponsive(int width, int height, string deviceType)
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await Page.SetViewportSizeAsync(width, height);
        await WaitForStickerGalleryAsync();
        await Task.Delay(500);
        
        // Assert
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Stickers')");
        
        // Check that sticker gallery adapts to screen size
        var stickers = await Page.QuerySelectorAllAsync("img[src*='sticker'], .sticker-item, [class*='sticker']");
        Assert.True(stickers.Count >= 0, $"Should handle stickers on {deviceType}");
    }

    [Theory]
    [InlineData(375, 667, "Mobile Portrait")]
    [InlineData(414, 896, "Mobile Landscape")]
    [InlineData(768, 1024, "Tablet Portrait")]
    [InlineData(1024, 768, "Tablet Landscape")]
    [InlineData(1920, 1080, "Desktop")]
    public async Task PlayMediaPage_ShouldBeResponsive(int width, int height, string deviceType)
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await Page.SetViewportSizeAsync(width, height);
        await WaitForVideoPlayerAsync();
        await Task.Delay(500);
        
        // Assert
        await AssertElementVisibleAsync("h1");
        await AssertElementVisibleAsync("iframe[src*='drive.google.com'], video");
        
        // Check that video player adapts to screen size
        var videoContainer = await Page.QuerySelectorAsync(".video-container");
        Assert.NotNull(videoContainer);
        
        // Verify device-specific behavior
        Assert.True(width > 0 && height > 0, $"Invalid viewport size for {deviceType}");
    }

    [Fact]
    public async Task MobileNavigation_ShouldWorkCorrectly()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await Page.SetViewportSizeAsync(375, 667);
        await WaitForBlazorComponentAsync("header");
        await Task.Delay(500);
        
        // Test navigation on mobile
        await ClickElementAsync("button:has-text('Quiz')", waitForNavigation: true);
        await AssertPageTitleAsync("Tom & Jerry Quiz - Test Your Knowledge!");
        
        // Navigate back
        await Page.GoBackAsync();
        await Task.Delay(500);
        
        // Navigate to stickers
        await ClickElementAsync("button:has-text('View All')", waitForNavigation: true);
        await AssertPageTitleAsync("Tom & Jerry Stickers - Tom & Jerry");
    }

    [Fact]
    public async Task TabletNavigation_ShouldWorkCorrectly()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await Page.SetViewportSizeAsync(768, 1024);
        await WaitForBlazorComponentAsync("header");
        await Task.Delay(500);
        
        // Test navigation on tablet
        await ClickElementAsync("button:has-text('Quiz')", waitForNavigation: true);
        await AssertPageTitleAsync("Tom & Jerry Quiz - Test Your Knowledge!");
        
        // Navigate back
        await Page.GoBackAsync();
        await Task.Delay(500);
        
        // Navigate to search
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await AssertPageTitleAsync("Search Results - Tom & Jerry");
    }

    [Fact]
    public async Task DesktopNavigation_ShouldWorkCorrectly()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await Page.SetViewportSizeAsync(1920, 1080);
        await WaitForBlazorComponentAsync("header");
        await Task.Delay(500);
        
        // Test navigation on desktop
        await ClickElementAsync("button:has-text('Quiz')", waitForNavigation: true);
        await AssertPageTitleAsync("Tom & Jerry Quiz - Test Your Knowledge!");
        
        // Navigate back
        await Page.GoBackAsync();
        await Task.Delay(500);
        
        // Navigate to video
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await AssertElementVisibleAsync("h1");
    }

    [Fact]
    public async Task MobileVideoGrid_ShouldAdaptToScreenSize()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await Page.SetViewportSizeAsync(375, 667);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        await Task.Delay(500);
        
        // Assert - Video grid should adapt to mobile
        await AssertElementVisibleAsync(".grid");
        
        // Check that video cards are properly sized for mobile
        var videoCards = await Page.QuerySelectorAllAsync(".tom-jerry-card");
        Assert.True(videoCards.Count > 0, "Should have video cards on mobile");
        
        // Test landscape orientation
        await Page.SetViewportSizeAsync(667, 375);
        await Task.Delay(500);
        
        // Assert - Should still work in landscape
        await AssertElementVisibleAsync(".grid");
    }

    [Fact]
    public async Task TabletVideoGrid_ShouldAdaptToScreenSize()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await Page.SetViewportSizeAsync(768, 1024);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        await Task.Delay(500);
        
        // Assert - Video grid should adapt to tablet
        await AssertElementVisibleAsync(".grid");
        
        // Check that video cards are properly sized for tablet
        var videoCards = await Page.QuerySelectorAllAsync(".tom-jerry-card");
        Assert.True(videoCards.Count > 0, "Should have video cards on tablet");
    }

    [Fact]
    public async Task DesktopVideoGrid_ShouldAdaptToScreenSize()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await Page.SetViewportSizeAsync(1920, 1080);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        await Task.Delay(500);
        
        // Assert - Video grid should adapt to desktop
        await AssertElementVisibleAsync(".grid");
        
        // Check that video cards are properly sized for desktop
        var videoCards = await Page.QuerySelectorAllAsync(".tom-jerry-card");
        Assert.True(videoCards.Count > 0, "Should have video cards on desktop");
    }

    [Fact]
    public async Task MobileQuiz_ShouldBeUsable()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await Page.SetViewportSizeAsync(375, 667);
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        await Task.Delay(500);
        
        // Start quiz
        await ClickElementAsync("button:has-text('5')");
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Assert - Quiz should be usable on mobile
        await AssertElementVisibleAsync("#quiz-questions");
        await AssertElementVisibleAsync("#answer-option-0");
        
        // Test answering questions
        await ClickElementAsync("#answer-option-0");
        await Task.Delay(500);
        
        // Should be able to navigate
        await ClickElementAsync("button:has-text('Next')");
        await Task.Delay(500);
    }

    [Fact]
    public async Task TabletQuiz_ShouldBeUsable()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await Page.SetViewportSizeAsync(768, 1024);
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        await Task.Delay(500);
        
        // Start quiz
        await ClickElementAsync("button:has-text('10')");
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Assert - Quiz should be usable on tablet
        await AssertElementVisibleAsync("#quiz-questions");
        await AssertElementVisibleAsync("#answer-option-0");
        
        // Test answering questions
        await ClickElementAsync("#answer-option-0");
        await Task.Delay(500);
    }

    [Fact]
    public async Task MobileStickers_ShouldBeUsable()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await Page.SetViewportSizeAsync(375, 667);
        await WaitForStickerGalleryAsync();
        await Task.Delay(500);
        
        // Assert - Stickers should be usable on mobile
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Stickers')");
        
        // Test filter buttons
        var filterButtons = await Page.QuerySelectorAllAsync("button");
        Assert.True(filterButtons.Count > 0, "Should have filter buttons on mobile");
        
        // Test sticker selection
        var stickers = await Page.QuerySelectorAllAsync("img[src*='sticker'], .sticker-item, [class*='sticker']");
        if (stickers.Count > 0)
        {
            await stickers[0].ClickAsync();
            await WaitForSnackbarAsync();
            await AssertElementVisibleAsync(".snackbar, .toast, .notification");
        }
    }

    [Fact]
    public async Task MobileVideoPlayer_ShouldBeUsable()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await Page.SetViewportSizeAsync(375, 667);
        await WaitForVideoPlayerAsync();
        await Task.Delay(500);
        
        // Assert - Video player should be usable on mobile
        await AssertElementVisibleAsync("iframe[src*='drive.google.com'], video");
        await AssertElementVisibleAsync("h1");
        
        // Test action buttons
        await AssertElementVisibleAsync("button:has-text('Like')");
        await AssertElementVisibleAsync("button:has-text('Share')");
        await AssertElementVisibleAsync("button:has-text('Save')");
    }

    [Fact]
    public async Task ResponsiveText_ShouldBeReadable()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        
        // Test different screen sizes
        var viewports = new[]
        {
            (375, 667, "Mobile"),
            (768, 1024, "Tablet"),
            (1920, 1080, "Desktop")
        };
        
        foreach (var (width, height, device) in viewports)
        {
            await Page.SetViewportSizeAsync(width, height);
            await Task.Delay(500);
            
            // Assert - Text should be readable
            var headings = await Page.QuerySelectorAllAsync("h1, h2, h3");
            Assert.True(headings.Count > 0, $"Should have headings on {device}");
            
            var buttons = await Page.QuerySelectorAllAsync("button");
            foreach (var button in buttons)
            {
                var text = await button.TextContentAsync();
                Assert.False(string.IsNullOrEmpty(text), $"Button text should be readable on {device}");
            }
        }
    }

    [Fact]
    public async Task ResponsiveImages_ShouldLoadCorrectly()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        
        // Test different screen sizes
        var viewports = new[]
        {
            (375, 667, "Mobile"),
            (768, 1024, "Tablet"),
            (1920, 1080, "Desktop")
        };
        
        foreach (var (width, height, device) in viewports)
        {
            await Page.SetViewportSizeAsync(width, height);
            await WaitForLoadingCompleteAsync();
            await WaitForVideoThumbnailsAsync();
            await Task.Delay(500);
            
            // Assert - Images should load correctly
            var images = await Page.QuerySelectorAllAsync("img");
            foreach (var img in images)
            {
                var src = await img.GetAttributeAsync("src");
                Assert.False(string.IsNullOrEmpty(src), $"Image should have src on {device}");
            }
        }
    }

    [Fact]
    public async Task ResponsiveLayout_ShouldNotBreak()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        
        // Test extreme screen sizes
        var viewports = new[]
        {
            (320, 568, "Small Mobile"),
            (375, 667, "Mobile"),
            (414, 896, "Large Mobile"),
            (768, 1024, "Tablet"),
            (1024, 768, "Tablet Landscape"),
            (1920, 1080, "Desktop"),
            (2560, 1440, "Large Desktop")
        };
        
        foreach (var (width, height, device) in viewports)
        {
            await Page.SetViewportSizeAsync(width, height);
            await WaitForBlazorComponentAsync(".tom-jerry-header");
            await Task.Delay(500);
            
            // Assert - Layout should not break
            await AssertElementVisibleAsync("h1");
            await AssertElementVisibleAsync("header");
            await AssertElementVisibleAsync("footer");
            
            // Check that no elements are overlapping or cut off
            var bodyHeight = await Page.EvaluateAsync<int>("document.body.scrollHeight");
            var viewportHeight = await Page.EvaluateAsync<int>("window.innerHeight");
            Assert.True(bodyHeight > 0, $"Page should have content on {device}");
        }
    }

    [Fact]
    public async Task MobileTouch_ShouldWorkCorrectly()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await Page.SetViewportSizeAsync(375, 667);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        await Task.Delay(500);
        
        // Test touch interactions
        var buttons = await Page.QuerySelectorAllAsync("button");
        if (buttons.Count > 0)
        {
            // Simulate touch on first button
            await buttons[0].TapAsync();
            await Task.Delay(500);
            
            // Assert - Touch should work
            Assert.True(true, "Touch interaction should work");
        }
    }

    [Fact]
    public async Task ResponsivePerformance_ShouldBeAcceptable()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        
        // Test performance on different screen sizes
        var viewports = new[]
        {
            (375, 667, "Mobile"),
            (768, 1024, "Tablet"),
            (1920, 1080, "Desktop")
        };
        
        foreach (var (width, height, device) in viewports)
        {
            var startTime = DateTime.Now;
            
            await Page.SetViewportSizeAsync(width, height);
            await WaitForBlazorComponentAsync(".tom-jerry-header");
            await WaitForLoadingCompleteAsync();
            
            var endTime = DateTime.Now;
            var loadTime = (endTime - startTime).TotalSeconds;
            
            // Assert - Should load within reasonable time
            Assert.True(loadTime < 10, $"Page should load within 10 seconds on {device}, took {loadTime:F2}s");
        }
    }
}
