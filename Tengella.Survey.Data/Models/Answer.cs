using System.ComponentModel.DataAnnotations;

namespace Tengella.Survey.Data.Models

{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public required Question Question { get; set; }
        public required string AnswerText { get; set; }

    }
}
