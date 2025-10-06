using System.ComponentModel.DataAnnotations;

namespace Basic_Project.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string Answer { get; set; }

        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}