using System.ComponentModel.DataAnnotations;

namespace Tengella.Survey.Data.Models
{
    public class Response
    {
        [Key]
        public int Id { get; set; }
        public required int QuestionId { get; set; }
        public required string Content { get; set; }
    }
}
