namespace Basic_Project.ViewModels
{
    public class QuizResultViewModel
    {
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }
        public int TotalScore { get; set; }
        public int MaxScore { get; set; }
        public double Percentage { get; set; }
        public List<QuestionResultViewModel> QuestionResults { get; set; } = new List<QuestionResultViewModel>();
    }

    public class QuestionResultViewModel
    {
        public string QuestionText { get; set; }
        public string UserAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public int Points { get; set; }
    }
}