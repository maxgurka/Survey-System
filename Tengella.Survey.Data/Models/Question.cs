using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tengella.Survey.Data.Models

{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public required string Content { get; set; }
        public virtual ICollection<Answer>? Answers { get; set; }

        [Required]
		[JsonIgnore]
		public virtual Survey? Survey { get; set; }
	}
}
