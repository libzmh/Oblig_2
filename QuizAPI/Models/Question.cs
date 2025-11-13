using System.ComponentModel.DataAnnotations;

namespace QuizAPI.Models
{
    public enum QuestionType
    {
        MultipleChoice = 0, 
        Checkbox = 1,       
        ShortAnswer = 2,     
        Dropdown = 3       
    }

    public class Question
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Question text is required")]
        [StringLength(500, MinimumLength = 1)]
        public string QuestionText { get; set; } = string.Empty;
        
        [Required]
        public QuestionType Type { get; set; } = QuestionType.MultipleChoice;
        
        
        [Required]
        public string OptionsJson { get; set; } = "[]";
        
        
        [Required]
        public string CorrectAnswersJson { get; set; } = "[]";
        
        [Range(1, 100)]
        public int Points { get; set; } = 1;
        
        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; }
    }
}