using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basic_Project.Models
{
    public class Quiz
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}

