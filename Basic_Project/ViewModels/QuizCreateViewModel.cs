using System.ComponentModel.DataAnnotations;
using Basic_Project.Models;

namespace Basic_Project.ViewModels
{
    public class QuizCreateViewModel
    {
        [Required(ErrorMessage = "Quiz title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        public List<QuestionCreateViewModel> Questions { get; set; } = new List<QuestionCreateViewModel>();
    }

    public class QuestionCreateViewModel
    {
        [Required(ErrorMessage = "Question text is required")]
        [StringLength(500, ErrorMessage = "Question text cannot exceed 500 characters")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Option A is required")]
        [StringLength(200)]
        public string OptionA { get; set; }

        [Required(ErrorMessage = "Option B is required")]
        [StringLength(200)]
        public string OptionB { get; set; }

        [StringLength(200)]
        public string? OptionC { get; set; }

        [StringLength(200)]
        public string? OptionD { get; set; }

        [Required(ErrorMessage = "Correct answer is required")]
        [RegularExpression("^[A-D]$", ErrorMessage = "Correct answer must be A, B, C, or D")]
        public string CorrectAnswer { get; set; }

        [Range(1, 100, ErrorMessage = "Points must be between 1 and 100")]
        public int Points { get; set; } = 1;
    }
}