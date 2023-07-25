using System.ComponentModel.DataAnnotations;

namespace Tengella.Survey.Data.Models

{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public required string Content { get; set; }
        public required ICollection<Answer> Answers { get; set; }

	}
}
