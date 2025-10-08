using System.ComponentModel.DataAnnotations;

namespace Basic_Project.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        public string Title { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        // Navigation property
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}