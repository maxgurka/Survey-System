using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tengella.Survey.Data.Models

{
	public enum RecipientType
	{
		Person,
		Company,
		Organization
	}
	public class Recipient
	{
		[Key]
		public int Id { get; set; }
		public required string Email { get; set; }
		public required string Name { get; set; }
		public required RecipientType Type { get; set; }
		public required string Identifier { get; set; }
		[JsonIgnore]
        public virtual ICollection<Respondent>? Respondents { get; set; }

    }
}
