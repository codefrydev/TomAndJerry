using TomAndJerry.Services;
using TomAndJerry.Model;
using TomAndJerry.Component;
using Microsoft.JSInterop;

namespace TomAndJerry.Pages;

public partial class Quiz : IDisposable
{
    private List<QuizQuestion> questions = new();
    private List<QuizAnswer> answers = new();
    private QuizResult? result;
    private QuizQuestion? currentQuestion;
    private int currentQuestionIndex = 0;
    private int questionCount = 10;
    private string selectedDifficulty = "all";
    private string selectedCategory = "all";
    private bool quizStarted = false;
    private bool showResults = false;
    private TimeSpan timeRemaining = TimeSpan.Zero;
    private DateTime quizStartTime;
    private Timer? timer;

    // Selection options
    private readonly List<SelectionOption> questionCountOptions = new()
    {
        new() { Value = 5, Label = "Quick Quiz", Emoji = "⚡" },
        new() { Value = 10, Label = "Standard", Emoji = "🎯" },
        new() { Value = 15, Label = "Challenging", Emoji = "🔥" },
        new() { Value = 20, Label = "Expert Level", Emoji = "🏆" }
    };

    private readonly List<SelectionOption> difficultyOptions = new()
    {
        new() { Value = "all", Label = "Mixed", Description = "All Levels", Emoji = "🎲" },
        new() { Value = "easy", Label = "Easy", Description = "Beginner", Emoji = "😊" },
        new() { Value = "medium", Label = "Medium", Description = "Intermediate", Emoji = "🤔" },
        new() { Value = "hard", Label = "Hard", Description = "Expert", Emoji = "😤" }
    };

    private readonly List<SelectionOption> categoryOptions = new()
    {
        new() { Value = "all", Label = "All", Emoji = "🌟" },
        new() { Value = "characters", Label = "Characters", Emoji = "🎭" },
        new() { Value = "history", Label = "History", Emoji = "📜" },
        new() { Value = "awards", Label = "Awards", Emoji = "🏆" },
        new() { Value = "general", Label = "General", Emoji = "🎪" }
    };

    public class SelectionOption
    {
        public object Value { get; set; } = 0;
        public string Label { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Emoji { get; set; } = string.Empty;
    }

    private async Task StartQuiz()
    {
        quizStarted = true;
        showResults = false;
        currentQuestionIndex = 0;
        answers.Clear();
        
        // Load questions based on settings
        if (selectedCategory == "all")
        {
            questions = await QuizService.GetQuizQuestionsAsync(questionCount, selectedDifficulty);
        }
        else
        {
            questions = await QuizService.GetQuestionsByCategoryAsync(selectedCategory, questionCount);
        }

        if (questions.Any())
        {
            currentQuestion = questions[0];
            quizStartTime = DateTime.Now;
            timeRemaining = TimeSpan.FromMinutes(questionCount * 0.5); // 30 seconds per question
            StartTimer();
            
            // Scroll to quiz questions section with smooth animation
            await Task.Delay(100); // Small delay to ensure DOM is updated
            await JSRuntime.InvokeVoidAsync("scrollToElement", "quiz-questions");
            
            // Focus on the first answer option for better accessibility
            await Task.Delay(300); // Wait for scroll animation to complete
            await JSRuntime.InvokeVoidAsync("focusFirstAnswerOption");
        }
    }

    private void StartTimer()
    {
        timer?.Dispose();
        timer = new Timer(async _ =>
        {
            timeRemaining = timeRemaining.Subtract(TimeSpan.FromSeconds(1));
            if (timeRemaining <= TimeSpan.Zero)
            {
                await SubmitQuiz();
            }
            await InvokeAsync(StateHasChanged);
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }

    private void SelectAnswer(int optionIndex)
    {
        if (currentQuestion == null) return;

        var existingAnswer = answers.FirstOrDefault(a => a.QuestionId == currentQuestion.Id);
        if (existingAnswer != null)
        {
            existingAnswer.SelectedAnswer = currentQuestion.Options[optionIndex];
            existingAnswer.IsCorrect = optionIndex == currentQuestion.CorrectAnswerIndex;
        }
        else
        {
            answers.Add(new QuizAnswer
            {
                QuestionId = currentQuestion.Id,
                Question = currentQuestion.Question,
                SelectedAnswer = currentQuestion.Options[optionIndex],
                CorrectAnswer = currentQuestion.Options[currentQuestion.CorrectAnswerIndex],
                IsCorrect = optionIndex == currentQuestion.CorrectAnswerIndex,
                Explanation = currentQuestion.Explanation
            });
        }

        StateHasChanged();
    }

    private async Task NextQuestion()
    {
        if (currentQuestionIndex < questions.Count - 1)
        {
            currentQuestionIndex++;
            currentQuestion = questions[currentQuestionIndex];
            StateHasChanged();
            
            // Scroll to top of quiz questions section
            await Task.Delay(50);
            await JSRuntime.InvokeVoidAsync("scrollToElement", "quiz-questions");
        }
    }

    private async Task PreviousQuestion()
    {
        if (currentQuestionIndex > 0)
        {
            currentQuestionIndex--;
            currentQuestion = questions[currentQuestionIndex];
            StateHasChanged();
            
            // Scroll to top of quiz questions section
            await Task.Delay(50);
            await JSRuntime.InvokeVoidAsync("scrollToElement", "quiz-questions");
        }
    }

    private async Task SubmitQuiz()
    {
        timer?.Dispose();
        var timeTaken = DateTime.Now - quizStartTime;
        result = await QuizService.CalculateResultAsync(answers, timeTaken);
        showResults = true;
        quizStarted = false;
        
        // Scroll to results section
        await Task.Delay(100); // Small delay to ensure DOM is updated
        await JSRuntime.InvokeVoidAsync("scrollToElement", "quiz-results");
    }

    private void RestartQuiz()
    {
        quizStarted = false;
        showResults = false;
        questions.Clear();
        answers.Clear();
        currentQuestion = null;
        currentQuestionIndex = 0;
        result = null;
        timer?.Dispose();
    }

    private void GoHome()
    {
        nav.NavigateTo("");
    }

    private void SelectQuestionCount(int count)
    {
        questionCount = count;
        StateHasChanged();
    }

    private void SelectDifficulty(string difficulty)
    {
        selectedDifficulty = difficulty;
        StateHasChanged();
    }

    private void SelectCategory(string category)
    {
        selectedCategory = category;
        StateHasChanged();
    }

    private string GetOptionClass(int optionIndex)
    {
        var answer = answers.FirstOrDefault(a => a.QuestionId == currentQuestion?.Id);
        if (answer == null) return "border-amber-300 hover:border-tom-blue hover:bg-amber-50";
        
        if (answer.SelectedAnswer == currentQuestion?.Options[optionIndex])
        {
            return "border-tom-blue bg-tom-blue/10 shadow-md";
        }
        
        return "border-amber-300 hover:border-tom-blue hover:bg-amber-50";
    }

    private string GetOptionIconClass(int optionIndex)
    {
        var answer = answers.FirstOrDefault(a => a.QuestionId == currentQuestion?.Id);
        if (answer == null) return "border-amber-300 text-amber-800";
        
        if (answer.SelectedAnswer == currentQuestion?.Options[optionIndex])
        {
            return "border-tom-blue bg-tom-blue text-white";
        }
        
        return "border-amber-300 text-amber-800";
    }

    private string GetResultEmoji()
    {
        if (result == null) return "🎯";
        
        return result.ScorePercentage switch
        {
            >= 90 => "🏆",
            >= 80 => "🌟",
            >= 70 => "👍",
            >= 60 => "😊",
            >= 50 => "📺",
            _ => "🎬"
        };
    }

    private string GetRandomCategory()
    {
        var categories = new[] { "classic", "playful", "general" };
        var random = new Random();
        return categories[random.Next(categories.Length)];
    }

    private string GetDifficultyEmoji()
    {
        return selectedDifficulty switch
        {
            "easy" => "😊",
            "medium" => "🤔",
            "hard" => "😤",
            _ => "🎲"
        };
    }

    private string GetDifficultyText()
    {
        return selectedDifficulty switch
        {
            "easy" => "Easy",
            "medium" => "Medium",
            "hard" => "Hard",
            _ => "Mixed"
        };
    }

    private string GetCategoryEmoji()
    {
        return selectedCategory switch
        {
            "characters" => "🎭",
            "history" => "📜",
            "awards" => "🏆",
            "general" => "🎪",
            _ => "🌟"
        };
    }

    private string GetCategoryText()
    {
        return selectedCategory switch
        {
            "characters" => "Characters",
            "history" => "History",
            "awards" => "Awards",
            "general" => "General",
            _ => "All"
        };
    }

    private double GetTimerPercentage()
    {
        if (timeRemaining <= TimeSpan.Zero) return 0;
        
        var totalTime = TimeSpan.FromMinutes(questionCount * 0.5); // 30 seconds per question
        return (timeRemaining.TotalSeconds / totalTime.TotalSeconds) * 100;
    }

    private string GetTimerBarClass()
    {
        var percentage = GetTimerPercentage();
        return percentage switch
        {
            > 50 => "shadow-lg",
            > 25 => "shadow-md",
            > 10 => "shadow-sm animate-pulse",
            _ => "shadow-sm animate-pulse"
        };
    }

    private string GetTimerBarAnimationClass()
    {
        var percentage = GetTimerPercentage();
        return percentage switch
        {
            > 25 => "timer-bar-animation",
            _ => "timer-bar-urgent timer-pulse"
        };
    }

    private string GetTimeStatus()
    {
        var percentage = GetTimerPercentage();
        return percentage switch
        {
            > 75 => "Plenty of Time! 😊",
            > 50 => "Good Pace! 👍",
            > 25 => "Hurry Up! ⚡",
            > 10 => "Almost Out! 😰",
            _ => "Time's Up! ⏰"
        };
    }

    private bool IsTimeRunningLow()
    {
        return GetTimerPercentage() <= 25;
    }

    public void Dispose()
    {
        timer?.Dispose();
    }
}
