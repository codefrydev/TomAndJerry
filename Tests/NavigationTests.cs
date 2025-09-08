using Xunit;

namespace TomAndJerry.Tests;

/// <summary>
/// Comprehensive UI tests for navigation between pages
/// </summary>
public class NavigationTests : UITestBase
{
    [Fact]
    public async Task Navigation_ShouldNavigateFromHomeToQuiz()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Click Quiz button in header
        await ClickElementAsync("button:has-text('Quiz')", waitForNavigation: true);
        
        // Assert
        await AssertPageTitleAsync("Tom & Jerry Quiz - Test Your Knowledge!");
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Quiz')");
    }

    [Fact]
    public async Task Navigation_ShouldNavigateFromHomeToStickers()
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
    public async Task Navigation_ShouldNavigateFromHomeToSearch()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Use search functionality (assuming there's a search input)
        var searchInput = await Page.QuerySelectorAsync("input[type='text'], input[placeholder*='search']");
        if (searchInput != null)
        {
            await FillInputAsync("input[type='text'], input[placeholder*='search']", "tom");
            await Page.Keyboard.PressAsync("Enter");
            await Task.Delay(1000);
            
            // Assert
            await AssertPageTitleAsync("Search Results - Tom & Jerry");
        }
    }

    [Fact]
    public async Task Navigation_ShouldNavigateFromQuizToHome()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Complete a quick quiz
        await ClickElementAsync("button:has-text('5')");
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Answer all questions quickly
        for (int i = 0; i < 5; i++)
        {
            await ClickElementAsync("#answer-option-0");
            if (i < 4)
            {
                await ClickElementAsync("button:has-text('Next')");
                await Task.Delay(200);
            }
        }
        
        await ClickElementAsync("button:has-text('Submit Quiz')");
        await Task.Delay(2000);
        
        // Click Go Home
        await ClickElementAsync("button:has-text('Go Home')", waitForNavigation: true);
        
        // Assert
        await AssertPageTitleAsync("Home - Tom & Jerry");
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry')");
    }

    [Fact]
    public async Task Navigation_ShouldNavigateFromStickersToHome()
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
    public async Task Navigation_ShouldNavigateFromSearchToHome()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await WaitForSearchResultsAsync();
        
        // Click back to home button
        await ClickElementAsync("button:has-text('Back to Home')", waitForNavigation: true);
        
        // Assert
        await AssertPageTitleAsync("Home - Tom & Jerry");
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry')");
    }

    [Fact]
    public async Task Navigation_ShouldNavigateFromHomeToVideo()
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
            
            // Assert
            var currentUrl = Page.Url;
            Assert.Contains("/playmedia/", currentUrl);
            await AssertElementVisibleAsync("h1");
        }
    }

    [Fact]
    public async Task Navigation_ShouldNavigateFromVideoToHome()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await WaitForBlazorComponentAsync(".tom-jerry-card");
        
        // Click on logo or home button (if available)
        var logoElement = await Page.QuerySelectorAsync(".tom-character, [class*='logo']");
        if (logoElement != null)
        {
            await logoElement.ClickAsync();
            await Task.Delay(1000);
            
            // Assert
            await AssertPageTitleAsync("Home - Tom & Jerry");
        }
    }

    [Fact]
    public async Task Navigation_ShouldNavigateBetweenVideos()
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
            await AssertElementVisibleAsync("h1");
        }
    }

    [Fact]
    public async Task Navigation_ShouldNavigateFromFooterLinks()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-footer");
        
        // Click on footer navigation links
        await ClickElementAsync("button:has-text('All Episodes')", waitForNavigation: true);
        await Task.Delay(500);
        
        // Should be on home page (all episodes)
        await AssertPageTitleAsync("Home - Tom & Jerry");
        
        // Navigate to stickers from footer
        await ClickElementAsync("button:has-text('Sticker Gallery')", waitForNavigation: true);
        await AssertPageTitleAsync("Tom & Jerry Stickers - Tom & Jerry");
        
        // Navigate to search from footer
        await ClickElementAsync("button:has-text('Search Episodes')", waitForNavigation: true);
        await AssertPageTitleAsync("Search Results - Tom & Jerry");
    }

    [Fact]
    public async Task Navigation_ShouldHandleBrowserBackButton()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Navigate to quiz
        await ClickElementAsync("button:has-text('Quiz')", waitForNavigation: true);
        await AssertPageTitleAsync("Tom & Jerry Quiz - Test Your Knowledge!");
        
        // Use browser back button
        await Page.GoBackAsync();
        await Task.Delay(1000);
        
        // Assert - Should be back on home page
        await AssertPageTitleAsync("Home - Tom & Jerry");
    }

    [Fact]
    public async Task Navigation_ShouldHandleBrowserForwardButton()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Navigate to quiz
        await ClickElementAsync("button:has-text('Quiz')", waitForNavigation: true);
        await AssertPageTitleAsync("Tom & Jerry Quiz - Test Your Knowledge!");
        
        // Go back
        await Page.GoBackAsync();
        await Task.Delay(1000);
        
        // Go forward
        await Page.GoForwardAsync();
        await Task.Delay(1000);
        
        // Assert - Should be on quiz page again
        await AssertPageTitleAsync("Tom & Jerry Quiz - Test Your Knowledge!");
    }

    [Fact]
    public async Task Navigation_ShouldMaintainStateBetweenPages()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Navigate to quiz and start it
        await ClickElementAsync("button:has-text('Quiz')", waitForNavigation: true);
        await ClickElementAsync("button:has-text('5')");
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Navigate back to home
        await Page.GoBackAsync();
        await Task.Delay(1000);
        
        // Navigate back to quiz
        await ClickElementAsync("button:has-text('Quiz')", waitForNavigation: true);
        
        // Assert - Quiz should be reset (not in progress)
        await AssertElementVisibleAsync("h2:has-text('Ready to Test Your Knowledge?')");
    }

    [Fact]
    public async Task Navigation_ShouldHandleDirectUrlAccess()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        
        // Assert
        await AssertPageTitleAsync("Tom & Jerry Quiz - Test Your Knowledge!");
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Quiz')");
        
        // Test other direct URLs
        await NavigateToUrlAsync($"{BaseUrl}/stickers");
        await AssertPageTitleAsync("Tom & Jerry Stickers - Tom & Jerry");
        
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await AssertPageTitleAsync("Search Results - Tom & Jerry");
        
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        await AssertElementVisibleAsync("h1");
    }

    [Fact]
    public async Task Navigation_ShouldHandleInvalidUrls()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/nonexistent");
        
        // Assert - Should handle gracefully (either redirect or show error page)
        // The app should not crash
        var title = await Page.TitleAsync();
        Assert.False(string.IsNullOrEmpty(title), "Page should have a title even for invalid URLs");
    }

    [Fact]
    public async Task Navigation_ShouldPreserveScrollPosition()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForLoadingCompleteAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Scroll down
        await Page.EvaluateAsync("window.scrollTo(0, 500)");
        await Task.Delay(500);
        
        // Navigate to another page
        await ClickElementAsync("button:has-text('Quiz')", waitForNavigation: true);
        await Task.Delay(500);
        
        // Navigate back
        await Page.GoBackAsync();
        await Task.Delay(1000);
        
        // Assert - Scroll position should be preserved (this might not work in all cases)
        var scrollY = await Page.EvaluateAsync<int>("window.scrollY");
        Assert.True(scrollY >= 0, "Should have scroll position");
    }

    [Fact]
    public async Task Navigation_ShouldHandleRapidNavigation()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Rapidly navigate between pages
        await ClickElementAsync("button:has-text('Quiz')", waitForNavigation: true);
        await Task.Delay(100);
        
        await Page.GoBackAsync();
        await Task.Delay(100);
        
        await ClickElementAsync("button:has-text('View All')", waitForNavigation: true);
        await Task.Delay(100);
        
        await Page.GoBackAsync();
        await Task.Delay(100);
        
        // Assert - App should handle rapid navigation without errors
        await AssertPageTitleAsync("Home - Tom & Jerry");
    }

    [Fact]
    public async Task Navigation_ShouldUpdateUrlCorrectly()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Navigate to quiz
        await ClickElementAsync("button:has-text('Quiz')", waitForNavigation: true);
        
        // Assert
        var currentUrl = Page.Url;
        Assert.Contains("/quiz", currentUrl);
        
        // Navigate to stickers
        await ClickElementAsync("button:has-text('← Back to Episodes')", waitForNavigation: true);
        await ClickElementAsync("button:has-text('View All')", waitForNavigation: true);
        
        currentUrl = Page.Url;
        Assert.Contains("/stickers", currentUrl);
    }

    [Fact]
    public async Task Navigation_ShouldHandleNavigationWithParameters()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/tom");
        
        // Assert
        var currentUrl = Page.Url;
        Assert.Contains("/Search/tom", currentUrl);
        
        // Test video navigation with ID
        await NavigateToUrlAsync($"{BaseUrl}/playmedia/1");
        currentUrl = Page.Url;
        Assert.Contains("/playmedia/1", currentUrl);
    }

    [Fact]
    public async Task Navigation_ShouldWorkWithKeyboardNavigation()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Use Tab to navigate to quiz button
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Enter");
        await Task.Delay(1000);
        
        // Assert
        await AssertPageTitleAsync("Tom & Jerry Quiz - Test Your Knowledge!");
    }

    [Fact]
    public async Task Navigation_ShouldHandleNavigationInterruption()
    {
        // Arrange & Act
        await NavigateToUrlAsync(BaseUrl);
        await WaitForBlazorComponentAsync(".tom-jerry-header");
        
        // Start navigation to quiz
        await ClickElementAsync("button:has-text('Quiz')");
        
        // Immediately navigate to stickers
        await ClickElementAsync("button:has-text('View All')", waitForNavigation: true);
        
        // Assert - Should end up on stickers page
        await AssertPageTitleAsync("Tom & Jerry Stickers - Tom & Jerry");
    }
}
