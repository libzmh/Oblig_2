using System.ComponentModel.DataAnnotations;

namespace QuizAPI.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        // Navigation property
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}