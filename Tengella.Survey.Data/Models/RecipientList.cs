using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tengella.Survey.Data.Models
{
	public class RecipientList
	{
		[Key]
		public int Id { get; set; }
		public required string Name { get; set; }
		public virtual ICollection<Recipient>? Recipients { get; set; }
	}
}
