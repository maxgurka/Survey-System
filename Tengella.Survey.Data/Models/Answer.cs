using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tengella.Survey.Data.Models

{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public required string Content { get; set; }
		[Required]
		[JsonIgnore]
		public virtual Question? Question { get; set; }
	}
}
