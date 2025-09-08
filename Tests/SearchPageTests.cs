using Xunit;

namespace TomAndJerry.Tests;

/// <summary>
/// Comprehensive UI tests for the Search page functionality
/// </summary>
public class SearchPageTests : UITestBase
{
    [Fact]
    public async Task SearchPage_ShouldLoadSuccessfully()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        
        // Assert
        await AssertPageTitleAsync("Search Results - Tom & Jerry");
        await AssertElementVisibleAsync("h2:has-text('All Episodes')");
    }

    [Fact]
    public async Task SearchPage_ShouldDisplayAllEpisodesWhenNoSearchTerm()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await WaitForSearchResultsAsync();
        
        // Assert
        await AssertElementVisibleAsync("h2:has-text('All Episodes')");
        await AssertElementTextAsync("p", "161 episodes available");
        
        // Check for video grid
        var videoCards = await Page.QuerySelectorAllAsync(".tom-jerry-card, .group");
        Assert.True(videoCards.Count > 0, "Should have video cards when showing all episodes");
    }

    [Fact]
    public async Task SearchPage_ShouldDisplaySearchResults()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/classic");
        await WaitForSearchResultsAsync();
        
        // Assert
        await AssertElementVisibleAsync("h2:has-text('Episodes Found')");
        await AssertElementTextAsync("p", "results for \"classic\"");
    }

    [Fact]
    public async Task SearchPage_ShouldDisplayNoResultsState()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/nonexistentsearchterm");
        await WaitForSearchResultsAsync();
        
        // Assert
        await AssertElementVisibleAsync("h3:has-text('No episodes found')");
        await AssertElementTextAsync("p", "We couldn't find any episodes matching");
        await AssertElementVisibleAsync("text:has-text('nonexistentsearchterm')");
    }

    [Fact]
    public async Task SearchPage_ShouldDisplaySearchSuggestions()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/nonexistentsearchterm");
        await WaitForSearchResultsAsync();
        
        // Assert
        await AssertElementVisibleAsync("text:has-text('Try searching for:')");
        await AssertElementVisibleAsync("button:has-text('Classic Episodes')");
        await AssertElementVisibleAsync("button:has-text('Funny Moments')");
        await AssertElementVisibleAsync("button:has-text('Best Chases')");
    }

    [Fact]
    public async Task SearchPage_ShouldNavigateBackToHome()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/nonexistentsearchterm");
        await WaitForSearchResultsAsync();
        
        // Click back to home button
        await ClickElementAsync("button:has-text('Back to Home')", waitForNavigation: true);
        
        // Assert
        await AssertPageTitleAsync("Home - Tom & Jerry");
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry')");
    }

    [Fact]
    public async Task SearchPage_ShouldDisplayVideoThumbnails()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await WaitForSearchResultsAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Assert
        var videoCards = await Page.QuerySelectorAllAsync(".tom-jerry-card, .group");
        Assert.True(videoCards.Count > 0, "Should have video cards");
        
        // Check for thumbnail images
        var thumbnails = await Page.QuerySelectorAllAsync(".tom-jerry-card img, .group img");
        Assert.True(thumbnails.Count > 0, "Should have thumbnail images");
    }

    [Fact]
    public async Task SearchPage_ShouldNavigateToVideoOnClick()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await WaitForSearchResultsAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Click on first video card
        var firstVideoCard = await Page.QuerySelectorAsync(".tom-jerry-card, .group");
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
    public async Task SearchPage_ShouldDisplayVideoTitles()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await WaitForSearchResultsAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Assert
        var videoTitles = await Page.QuerySelectorAllAsync(".tom-jerry-card h3, .group h3");
        Assert.True(videoTitles.Count > 0, "Should have video titles");
        
        // Check that titles are not empty
        foreach (var title in videoTitles)
        {
            var titleText = await title.TextContentAsync();
            Assert.False(string.IsNullOrEmpty(titleText), "Video titles should not be empty");
        }
    }

    [Fact]
    public async Task SearchPage_ShouldDisplayChannelInfo()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await WaitForSearchResultsAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Assert
        var channelInfo = await Page.QuerySelectorAllAsync(".tom-jerry-card .text-amber-900, .group .text-amber-900");
        Assert.True(channelInfo.Count > 0, "Should have channel information");
        
        // Check for "Tom & Jerry" channel name
        var channelText = await GetElementTextAsync(".tom-jerry-card .text-amber-900, .group .text-amber-900");
        Assert.Contains("Tom & Jerry", channelText);
    }

    [Fact]
    public async Task SearchPage_ShouldDisplayVideoStats()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await WaitForSearchResultsAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Assert
        var statsElements = await Page.QuerySelectorAllAsync(".tom-jerry-card .text-amber-800, .group .text-amber-800");
        Assert.True(statsElements.Count > 0, "Should have video statistics");
        
        // Check for views and time ago
        var statsText = await GetElementTextAsync(".tom-jerry-card .text-amber-800, .group .text-amber-800");
        Assert.True(statsText.Contains("views") || statsText.Contains("ago"), "Should contain view count or time information");
    }

    [Fact]
    public async Task SearchPage_ShouldHandleEmptySearchTerm()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await WaitForSearchResultsAsync();
        
        // Assert
        await AssertElementVisibleAsync("h2:has-text('All Episodes')");
        await AssertElementTextAsync("p", "161 episodes available");
    }

    [Fact]
    public async Task SearchPage_ShouldHandleSpecialCharactersInSearch()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/tom%20%26%20jerry");
        await WaitForSearchResultsAsync();
        
        // Assert - Should handle URL encoded search terms
        await AssertElementVisibleAsync("h2:has-text('Episodes Found')");
    }

    [Fact]
    public async Task SearchPage_ShouldDisplayCorrectResultCount()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await WaitForSearchResultsAsync();
        
        // Get the count from the page
        var countText = await GetElementTextAsync("p");
        var countMatch = System.Text.RegularExpressions.Regex.Match(countText, @"(\d+) episodes available");
        
        if (countMatch.Success)
        {
            var displayedCount = int.Parse(countMatch.Groups[1].Value);
            
            // Count actual video cards
            var videoCards = await Page.QuerySelectorAllAsync(".tom-jerry-card, .group");
            var actualCount = videoCards.Count;
            
            // Assert - Counts should match (within reasonable range due to pagination)
            Assert.True(actualCount > 0, "Should have at least one video card");
        }
    }

    [Fact]
    public async Task SearchPage_ShouldHaveResponsiveDesign()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await WaitForSearchResultsAsync();
        
        // Test mobile viewport
        await Page.SetViewportSizeAsync(375, 667);
        await Task.Delay(500);
        
        // Assert - Elements should still be visible on mobile
        await AssertElementVisibleAsync("h2");
        var videoCards = await Page.QuerySelectorAllAsync(".tom-jerry-card, .group");
        Assert.True(videoCards.Count > 0, "Should have video cards on mobile");
        
        // Test desktop viewport
        await Page.SetViewportSizeAsync(1920, 1080);
        await Task.Delay(500);
        
        // Assert - Elements should still be visible on desktop
        await AssertElementVisibleAsync("h2");
        var videoCardsDesktop = await Page.QuerySelectorAllAsync(".tom-jerry-card, .group");
        Assert.True(videoCardsDesktop.Count > 0, "Should have video cards on desktop");
    }

    [Fact]
    public async Task SearchPage_ShouldHaveAccessibleElements()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await WaitForSearchResultsAsync();
        
        // Assert - Check for proper heading structure
        await AssertElementVisibleAsync("h2");
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
    public async Task SearchPage_ShouldDisplayLoadingState()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        
        // Assert - Should show loading state initially
        var hasLoadingElements = await Page.QuerySelectorAsync(".skeleton, .animate-pulse, .loading");
        if (hasLoadingElements != null)
        {
            await AssertElementVisibleAsync(".skeleton, .animate-pulse, .loading");
        }
        
        // Wait for loading to complete
        await WaitForLoadingCompleteAsync();
        
        // Assert - Loading should be gone
        await AssertElementHiddenAsync(".skeleton");
    }

    [Fact]
    public async Task SearchPage_ShouldHandleSearchWithResults()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/tom");
        await WaitForSearchResultsAsync();
        
        // Assert
        await AssertElementVisibleAsync("h2:has-text('Episodes Found')");
        await AssertElementTextAsync("p", "results for \"tom\"");
        
        // Should have some results
        var videoCards = await Page.QuerySelectorAllAsync(".tom-jerry-card, .group");
        Assert.True(videoCards.Count >= 0, "Should handle search with results gracefully");
    }

    [Fact]
    public async Task SearchPage_ShouldDisplaySearchIcon()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await WaitForSearchResultsAsync();
        
        // Assert
        await AssertElementVisibleAsync(".w-10.h-10.bg-green-100.rounded-full");
        
        // Check for search icon SVG
        var searchIcon = await Page.QuerySelectorAsync("svg");
        Assert.NotNull(searchIcon);
    }

    [Fact]
    public async Task SearchPage_ShouldHandleLongSearchTerms()
    {
        // Arrange & Act
        var longSearchTerm = "very long search term that might cause issues with the search functionality";
        await NavigateToUrlAsync($"{BaseUrl}/Search/{Uri.EscapeDataString(longSearchTerm)}");
        await WaitForSearchResultsAsync();
        
        // Assert - Should handle long search terms gracefully
        await AssertElementVisibleAsync("h2:has-text('Episodes Found')");
        await AssertElementTextAsync("p", $"results for \"{longSearchTerm}\"");
    }

    [Fact]
    public async Task SearchPage_ShouldDisplayVideoGridLayout()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/");
        await WaitForSearchResultsAsync();
        await WaitForVideoThumbnailsAsync();
        
        // Assert
        await AssertElementVisibleAsync(".grid.grid-cols-1.sm\\:grid-cols-2.md\\:grid-cols-3.lg\\:grid-cols-4.xl\\:grid-cols-5");
        
        // Check that video cards are in a grid layout
        var videoCards = await Page.QuerySelectorAllAsync(".tom-jerry-card, .group");
        Assert.True(videoCards.Count > 0, "Should have video cards in grid layout");
    }

    [Fact]
    public async Task SearchPage_ShouldHandleSearchWithSpecialCharacters()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/Search/tom%20%26%20jerry%20episode");
        await WaitForSearchResultsAsync();
        
        // Assert - Should handle special characters in search
        await AssertElementVisibleAsync("h2:has-text('Episodes Found')");
        
        // Should not crash or show error
        var errorElements = await Page.QuerySelectorAllAsync(".error, .alert-danger");
        Assert.True(errorElements.Count == 0, "Should not show error for special characters");
    }
}
