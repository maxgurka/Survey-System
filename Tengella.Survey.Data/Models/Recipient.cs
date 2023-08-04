using System.ComponentModel.DataAnnotations;

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

	}
}
