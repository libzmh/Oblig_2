using System.ComponentModel.DataAnnotations;

namespace Basic_Project.Models
{
    public class Question
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Question text is required")]
        [StringLength(500)]
        public string QuestionText { get; set; }
        
        [Required]
        [StringLength(200)]
        public string OptionA { get; set; }
        
        [Required]
        [StringLength(200)]
        public string OptionB { get; set; }
        
        [StringLength(200)]
        public string? OptionC { get; set; }
        
        [StringLength(200)]
        public string? OptionD { get; set; }
        
        [Required]
        [StringLength(1)]
        public string CorrectAnswer { get; set; } // "A", "B", "C", or "D"
        
        [Range(1, 100)]
        public int Points { get; set; } = 1;
        
        // Foreign key
        public int QuizId { get; set; }
        
        // Navigation property
        public Quiz Quiz { get; set; }
    }
}