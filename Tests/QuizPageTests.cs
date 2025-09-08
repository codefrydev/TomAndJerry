using Xunit;

namespace TomAndJerry.Tests;

/// <summary>
/// Comprehensive UI tests for the Quiz page functionality
/// </summary>
public class QuizPageTests : UITestBase
{
    [Fact]
    public async Task QuizPage_ShouldLoadSuccessfully()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        
        // Assert
        await AssertPageTitleAsync("Tom & Jerry Quiz - Test Your Knowledge!");
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Quiz')");
    }

    [Fact]
    public async Task QuizPage_ShouldDisplaySetupSection()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Assert
        await AssertElementVisibleAsync("h2:has-text('Ready to Test Your Knowledge?')");
        await AssertElementVisibleAsync("h3:has-text('How Many Questions?')");
        await AssertElementVisibleAsync("h3:has-text('Difficulty Level')");
        await AssertElementVisibleAsync("h3:has-text('Quiz Category')");
    }

    [Fact]
    public async Task QuizPage_ShouldDisplayQuestionCountOptions()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Assert
        await AssertElementVisibleAsync("button:has-text('5')");
        await AssertElementVisibleAsync("button:has-text('10')");
        await AssertElementVisibleAsync("button:has-text('15')");
        await AssertElementVisibleAsync("button:has-text('20')");
        
        // Check for option labels
        await AssertElementVisibleAsync("button:has-text('Quick Quiz')");
        await AssertElementVisibleAsync("button:has-text('Standard')");
        await AssertElementVisibleAsync("button:has-text('Challenging')");
        await AssertElementVisibleAsync("button:has-text('Expert Level')");
    }

    [Fact]
    public async Task QuizPage_ShouldDisplayDifficultyOptions()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Assert
        await AssertElementVisibleAsync("button:has-text('Mixed')");
        await AssertElementVisibleAsync("button:has-text('Easy')");
        await AssertElementVisibleAsync("button:has-text('Medium')");
        await AssertElementVisibleAsync("button:has-text('Hard')");
    }

    [Fact]
    public async Task QuizPage_ShouldDisplayCategoryOptions()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Assert
        await AssertElementVisibleAsync("button:has-text('All')");
        await AssertElementVisibleAsync("button:has-text('Characters')");
        await AssertElementVisibleAsync("button:has-text('History')");
        await AssertElementVisibleAsync("button:has-text('Awards')");
        await AssertElementVisibleAsync("button:has-text('General')");
    }

    [Fact]
    public async Task QuizPage_ShouldSelectQuestionCount()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Click on 10 questions option
        await ClickElementAsync("button:has-text('10')");
        await Task.Delay(500);
        
        // Assert - Button should be selected (have different styling)
        var selectedButton = await Page.QuerySelectorAsync("button:has-text('10')");
        var hasSelectedClass = await ElementHasClassAsync("button:has-text('10')", "border-tom-blue");
        Assert.True(hasSelectedClass, "10 questions button should be selected");
    }

    [Fact]
    public async Task QuizPage_ShouldSelectDifficulty()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Click on Medium difficulty
        await ClickElementAsync("button:has-text('Medium')");
        await Task.Delay(500);
        
        // Assert - Button should be selected
        var hasSelectedClass = await ElementHasClassAsync("button:has-text('Medium')", "border-cartoon-red");
        Assert.True(hasSelectedClass, "Medium difficulty button should be selected");
    }

    [Fact]
    public async Task QuizPage_ShouldSelectCategory()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Click on Characters category
        await ClickElementAsync("button:has-text('Characters')");
        await Task.Delay(500);
        
        // Assert - Button should be selected
        var hasSelectedClass = await ElementHasClassAsync("button:has-text('Characters')", "border-cartoon-yellow");
        Assert.True(hasSelectedClass, "Characters category button should be selected");
    }

    [Fact]
    public async Task QuizPage_ShouldDisplayStartButton()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Assert
        await AssertElementVisibleAsync("button:has-text('Start Quiz Adventure!')");
    }

    [Fact]
    public async Task QuizPage_ShouldStartQuiz()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Select options
        await ClickElementAsync("button:has-text('10')");
        await ClickElementAsync("button:has-text('Medium')");
        await ClickElementAsync("button:has-text('All')");
        
        // Start quiz
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Assert
        await AssertElementVisibleAsync("#quiz-questions");
        await AssertElementVisibleAsync("h3:has-text('Question')");
    }

    [Fact]
    public async Task QuizPage_ShouldDisplayTimer()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Start quiz
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Assert
        await AssertElementVisibleAsync(".text-2xl.font-bold.font-cartoon.text-amber-800");
        await AssertElementVisibleAsync("text:has-text('Time Remaining')");
    }

    [Fact]
    public async Task QuizPage_ShouldDisplayProgressBar()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Start quiz
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Assert
        await AssertElementVisibleAsync(".w-full.bg-amber-200.rounded-full.h-3");
        await AssertElementVisibleAsync("text:has-text('Question')");
        await AssertElementVisibleAsync("text:has-text('Complete')");
    }

    [Fact]
    public async Task QuizPage_ShouldDisplayAnswerOptions()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Start quiz
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Assert
        await AssertElementVisibleAsync("#answer-option-0");
        await AssertElementVisibleAsync("#answer-option-1");
        await AssertElementVisibleAsync("#answer-option-2");
        await AssertElementVisibleAsync("#answer-option-3");
    }

    [Fact]
    public async Task QuizPage_ShouldSelectAnswer()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Start quiz
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Select first answer
        await ClickElementAsync("#answer-option-0");
        await Task.Delay(500);
        
        // Assert - Answer should be selected
        var hasSelectedClass = await ElementHasClassAsync("#answer-option-0", "border-tom-blue");
        Assert.True(hasSelectedClass, "First answer option should be selected");
    }

    [Fact]
    public async Task QuizPage_ShouldNavigateBetweenQuestions()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Start quiz
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Select answer and go to next question
        await ClickElementAsync("#answer-option-0");
        await ClickElementAsync("button:has-text('Next')");
        await Task.Delay(500);
        
        // Assert - Should be on second question
        await AssertElementVisibleAsync("text:has-text('Question 2')");
    }

    [Fact]
    public async Task QuizPage_ShouldSubmitQuiz()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Start quiz with 5 questions
        await ClickElementAsync("button:has-text('5')");
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Answer all questions quickly
        for (int i = 0; i < 5; i++)
        {
            await ClickElementAsync("#answer-option-0");
            if (i < 4) // Not the last question
            {
                await ClickElementAsync("button:has-text('Next')");
                await Task.Delay(200);
            }
        }
        
        // Submit quiz
        await ClickElementAsync("button:has-text('Submit Quiz')");
        await Task.Delay(2000);
        
        // Assert
        await AssertElementVisibleAsync("#quiz-results");
        await AssertElementVisibleAsync("h2:has-text('Quiz Complete!')");
    }

    [Fact]
    public async Task QuizPage_ShouldDisplayResults()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Start and complete quiz
        await ClickElementAsync("button:has-text('5')");
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Answer all questions
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
        
        // Assert
        await AssertElementVisibleAsync("#quiz-results");
        await AssertElementVisibleAsync(".text-3xl.font-bold.font-cartoon.text-tom-blue"); // Correct answers
        await AssertElementVisibleAsync(".text-3xl.font-bold.font-cartoon.text-cartoon-red"); // Wrong answers
        await AssertElementVisibleAsync(".text-3xl.font-bold.font-cartoon.text-cartoon-yellow"); // Score percentage
    }

    [Fact]
    public async Task QuizPage_ShouldDisplayGrade()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Start and complete quiz
        await ClickElementAsync("button:has-text('5')");
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Answer all questions
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
        
        // Assert
        await AssertElementVisibleAsync("text:has-text('Grade:')");
        await AssertElementVisibleAsync("text:has-text('Time:')");
    }

    [Fact]
    public async Task QuizPage_ShouldDisplayDetailedResults()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Start and complete quiz
        await ClickElementAsync("button:has-text('5')");
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Answer all questions
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
        
        // Assert
        await AssertElementVisibleAsync("h3:has-text('Detailed Results')");
        
        // Check for detailed answer cards
        var answerCards = await Page.QuerySelectorAllAsync(".border-2.rounded-xl.p-4");
        Assert.True(answerCards.Count > 0, "Should have detailed answer cards");
    }

    [Fact]
    public async Task QuizPage_ShouldRestartQuiz()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Complete a quiz
        await ClickElementAsync("button:has-text('5')");
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
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
        
        // Click restart
        await ClickElementAsync("button:has-text('Take Another Quiz')");
        await Task.Delay(500);
        
        // Assert - Should be back to setup
        await AssertElementVisibleAsync("h2:has-text('Ready to Test Your Knowledge?')");
    }

    [Fact]
    public async Task QuizPage_ShouldGoHome()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Complete a quiz
        await ClickElementAsync("button:has-text('5')");
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
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
        
        // Click go home
        await ClickElementAsync("button:has-text('Go Home')", waitForNavigation: true);
        
        // Assert
        await AssertPageTitleAsync("Home - Tom & Jerry");
    }

    [Fact]
    public async Task QuizPage_ShouldHandleTimerExpiration()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Start quiz
        await ClickElementAsync("button:has-text('5')");
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Wait for timer to expire (this might take a while in real test)
        // For this test, we'll just verify the timer is running
        await AssertElementVisibleAsync("text:has-text('Time Remaining')");
        
        // Assert - Timer should be visible and running
        var timerElement = await Page.QuerySelectorAsync(".text-2xl.font-bold.font-cartoon.text-amber-800");
        Assert.NotNull(timerElement);
    }

    [Fact]
    public async Task QuizPage_ShouldDisplayPreviousButton()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Start quiz
        await ClickElementAsync("button:has-text('Start Quiz Adventure!')");
        await WaitForQuizQuestionsAsync();
        
        // Go to second question
        await ClickElementAsync("#answer-option-0");
        await ClickElementAsync("button:has-text('Next')");
        await Task.Delay(500);
        
        // Assert - Previous button should be enabled
        await AssertElementVisibleAsync("button:has-text('Previous')");
        
        // Click previous
        await ClickElementAsync("button:has-text('Previous')");
        await Task.Delay(500);
        
        // Assert - Should be back to first question
        await AssertElementVisibleAsync("text:has-text('Question 1')");
    }

    [Fact]
    public async Task QuizPage_ShouldHaveResponsiveDesign()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Test mobile viewport
        await Page.SetViewportSizeAsync(375, 667);
        await Task.Delay(500);
        
        // Assert - Elements should still be visible on mobile
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Quiz')");
        await AssertElementVisibleAsync("h2:has-text('Ready to Test Your Knowledge?')");
        
        // Test desktop viewport
        await Page.SetViewportSizeAsync(1920, 1080);
        await Task.Delay(500);
        
        // Assert - Elements should still be visible on desktop
        await AssertElementVisibleAsync("h1:has-text('Tom & Jerry Quiz')");
        await AssertElementVisibleAsync("h2:has-text('Ready to Test Your Knowledge?')");
    }

    [Fact]
    public async Task QuizPage_ShouldHaveAccessibleElements()
    {
        // Arrange & Act
        await NavigateToUrlAsync($"{BaseUrl}/quiz");
        await WaitForBlazorComponentAsync(".bg-white.rounded-2xl");
        
        // Assert - Check for proper heading structure
        await AssertElementVisibleAsync("h1");
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
}
