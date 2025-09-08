namespace TomAndJerry.Model
{
    public class QuizResult
    {
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public double ScorePercentage { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public List<QuizAnswer> Answers { get; set; } = new List<QuizAnswer>();
        public DateTime CompletedAt { get; set; } = DateTime.Now;
        public string Grade { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class QuizAnswer
    {
        public int QuestionId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string SelectedAnswer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public string Explanation { get; set; } = string.Empty;
    }
}
