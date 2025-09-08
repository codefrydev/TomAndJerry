using TomAndJerry.Model;

namespace TomAndJerry.Services
{
    public interface IQuizService
    {
        Task<List<QuizQuestion>> GetQuizQuestionsAsync(int count = 10, string difficulty = "all");
        Task<QuizQuestion?> GetQuestionByIdAsync(int id);
        Task<QuizResult> CalculateResultAsync(List<QuizAnswer> answers, TimeSpan timeTaken);
        Task<List<QuizQuestion>> GetQuestionsByCategoryAsync(string category, int count = 10);
        Task<List<string>> GetAvailableCategoriesAsync();
        Task<List<string>> GetAvailableDifficultiesAsync();
    }
}
