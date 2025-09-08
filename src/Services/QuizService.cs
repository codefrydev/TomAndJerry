using TomAndJerry.Model;

namespace TomAndJerry.Services
{
    public class QuizService : IQuizService
    {
        private readonly List<QuizQuestion> _questions;

        public QuizService()
        {
            _questions = InitializeQuestions();
        }

        public async Task<List<QuizQuestion>> GetQuizQuestionsAsync(int count = 10, string difficulty = "all")
        {
            await Task.Delay(100); // Simulate async operation

            var filteredQuestions = difficulty == "all" 
                ? _questions 
                : _questions.Where(q => q.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase)).ToList();

            var random = new Random();
            return filteredQuestions.OrderBy(x => random.Next()).Take(count).ToList();
        }

        public async Task<QuizQuestion?> GetQuestionByIdAsync(int id)
        {
            await Task.Delay(50);
            return _questions.FirstOrDefault(q => q.Id == id);
        }

        public async Task<QuizResult> CalculateResultAsync(List<QuizAnswer> answers, TimeSpan timeTaken)
        {
            await Task.Delay(100);

            var correctAnswers = answers.Count(a => a.IsCorrect);
            var wrongAnswers = answers.Count - correctAnswers;
            var scorePercentage = answers.Count > 0 ? (double)correctAnswers / answers.Count * 100 : 0;

            var grade = GetGrade(scorePercentage);
            var message = GetGradeMessage(grade, scorePercentage);

            return new QuizResult
            {
                TotalQuestions = answers.Count,
                CorrectAnswers = correctAnswers,
                WrongAnswers = wrongAnswers,
                ScorePercentage = scorePercentage,
                TimeTaken = timeTaken,
                Answers = answers,
                Grade = grade,
                Message = message
            };
        }

        public async Task<List<QuizQuestion>> GetQuestionsByCategoryAsync(string category, int count = 10)
        {
            await Task.Delay(100);
            var filteredQuestions = _questions.Where(q => q.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            var random = new Random();
            return filteredQuestions.OrderBy(x => random.Next()).Take(count).ToList();
        }

        public async Task<List<string>> GetAvailableCategoriesAsync()
        {
            await Task.Delay(50);
            return _questions.Select(q => q.Category).Distinct().OrderBy(c => c).ToList();
        }

        public async Task<List<string>> GetAvailableDifficultiesAsync()
        {
            await Task.Delay(50);
            return _questions.Select(q => q.Difficulty).Distinct().OrderBy(d => d).ToList();
        }

        private string GetGrade(double scorePercentage)
        {
            return scorePercentage switch
            {
                >= 90 => "A+",
                >= 80 => "A",
                >= 70 => "B",
                >= 60 => "C",
                >= 50 => "D",
                _ => "F"
            };
        }

        private string GetGradeMessage(string grade, double scorePercentage)
        {
            return grade switch
            {
                "A+" => "Outstanding! You're a true Tom & Jerry expert! 🏆",
                "A" => "Excellent! You know your Tom & Jerry very well! 🌟",
                "B" => "Great job! You have good knowledge of Tom & Jerry! 👍",
                "C" => "Not bad! You know some Tom & Jerry facts! 😊",
                "D" => "Keep watching! You'll learn more Tom & Jerry! 📺",
                _ => "Time to binge-watch some Tom & Jerry episodes! 🎬"
            };
        }

        private List<QuizQuestion> InitializeQuestions()
        {
            return new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Id = 1,
                    Question = "What year did the first Tom and Jerry cartoon premiere?",
                    Options = new List<string> { "1939", "1940", "1941", "1942" },
                    CorrectAnswerIndex = 1,
                    Explanation = "The first Tom and Jerry cartoon, 'Puss Gets the Boot', premiered in 1940.",
                    Category = "history",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 2,
                    Question = "What is Tom's full name?",
                    Options = new List<string> { "Thomas Cat", "Tom Cat", "Thomas", "Tom" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Tom's full name is Tom Cat, though he's often just called Tom.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 3,
                    Question = "How many Academy Awards did Tom and Jerry win?",
                    Options = new List<string> { "5", "6", "7", "8" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Tom and Jerry won 7 Academy Awards for Best Animated Short Film.",
                    Category = "awards",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 4,
                    Question = "What is Jerry's favorite food?",
                    Options = new List<string> { "Crackers", "Cheese", "Bread", "Nuts" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Jerry's favorite food is cheese, which often gets him into trouble with Tom.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 5,
                    Question = "Who created Tom and Jerry?",
                    Options = new List<string> { "Walt Disney", "William Hanna and Joseph Barbera", "Chuck Jones", "Tex Avery" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Tom and Jerry were created by William Hanna and Joseph Barbera at MGM.",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 6,
                    Question = "What color is Tom?",
                    Options = new List<string> { "Black", "Orange", "Blue-grey", "White" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Tom is typically blue-grey in color, though this can vary in different episodes.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 7,
                    Question = "In which decade did Tom and Jerry cartoons stop being produced by MGM?",
                    Options = new List<string> { "1950s", "1960s", "1970s", "1980s" },
                    CorrectAnswerIndex = 1,
                    Explanation = "MGM stopped producing Tom and Jerry cartoons in the 1960s, specifically in 1958.",
                    Category = "history",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 8,
                    Question = "What is the name of Tom and Jerry's owner?",
                    Options = new List<string> { "Mammy Two Shoes", "Spike", "Tyke", "They don't have an owner" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Mammy Two Shoes was Tom and Jerry's owner in the early cartoons, though she was later replaced.",
                    Category = "characters",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 9,
                    Question = "Which character is Tom's rival for Jerry's attention?",
                    Options = new List<string> { "Spike", "Butch", "Tuffy", "Nibbles" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Butch is another cat who often competes with Tom for Jerry's attention.",
                    Category = "characters",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 10,
                    Question = "What is the name of Jerry's nephew?",
                    Options = new List<string> { "Tuffy", "Nibbles", "Jerry Jr.", "Little Jerry" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Nibbles is Jerry's nephew who often appears in the cartoons.",
                    Category = "characters",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 11,
                    Question = "Which studio originally produced Tom and Jerry?",
                    Options = new List<string> { "Warner Bros.", "Disney", "MGM", "Paramount" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Tom and Jerry were originally produced by MGM (Metro-Goldwyn-Mayer).",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 12,
                    Question = "What is the typical length of a Tom and Jerry cartoon?",
                    Options = new List<string> { "5-7 minutes", "10-15 minutes", "20-30 minutes", "45-60 minutes" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Most Tom and Jerry cartoons are short films, typically 5-7 minutes long.",
                    Category = "general",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 13,
                    Question = "Which character is known for being a bulldog?",
                    Options = new List<string> { "Spike", "Butch", "Tom", "Jerry" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Spike is the bulldog character who often protects Jerry from Tom.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 14,
                    Question = "What is the name of Spike's son?",
                    Options = new List<string> { "Tyke", "Tuffy", "Nibbles", "Spike Jr." },
                    CorrectAnswerIndex = 0,
                    Explanation = "Tyke is Spike's son who often appears with his father.",
                    Category = "characters",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 15,
                    Question = "In which cartoon did Tom and Jerry first appear?",
                    Options = new List<string> { "Puss Gets the Boot", "The Midnight Snack", "Fraidy Cat", "Mouse Trouble" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Tom and Jerry first appeared in 'Puss Gets the Boot' in 1940.",
                    Category = "history",
                    Difficulty = "medium"
                },
                new QuizQuestion
                {
                    Id = 16,
                    Question = "What type of animal is Jerry?",
                    Options = new List<string> { "Rat", "Mouse", "Hamster", "Gerbil" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Jerry is a mouse, which is why Tom (the cat) is always trying to catch him.",
                    Category = "characters",
                    Difficulty = "easy"
                },
                new QuizQuestion
                {
                    Id = 17,
                    Question = "Which decade saw the revival of Tom and Jerry cartoons?",
                    Options = new List<string> { "1970s", "1980s", "1990s", "2000s" },
                    CorrectAnswerIndex = 2,
                    Explanation = "Tom and Jerry cartoons were revived in the 1990s with new episodes and movies.",
                    Category = "history",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 18,
                    Question = "What is the name of the character who is a little mouse and Jerry's friend?",
                    Options = new List<string> { "Tuffy", "Nibbles", "Little Jerry", "Mouse" },
                    CorrectAnswerIndex = 0,
                    Explanation = "Tuffy is a little mouse who is often Jerry's friend and companion.",
                    Category = "characters",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 19,
                    Question = "Which Tom and Jerry cartoon won the first Academy Award?",
                    Options = new List<string> { "The Yankee Doodle Mouse", "Mouse Trouble", "Quiet Please!", "The Cat Concerto" },
                    CorrectAnswerIndex = 0,
                    Explanation = "'The Yankee Doodle Mouse' won the first Academy Award for Tom and Jerry in 1943.",
                    Category = "awards",
                    Difficulty = "hard"
                },
                new QuizQuestion
                {
                    Id = 20,
                    Question = "What is the typical relationship between Tom and Jerry?",
                    Options = new List<string> { "Best friends", "Enemies", "Brothers", "Neighbors" },
                    CorrectAnswerIndex = 1,
                    Explanation = "Tom and Jerry are typically enemies, with Tom always trying to catch Jerry.",
                    Category = "general",
                    Difficulty = "easy"
                }
            };
        }
    }
}
