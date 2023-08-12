using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tengella.Survey.Data.Models
{
    public class Respondent
    {
        [Key]
        public int Id { get; set; }
		public virtual ICollection<Response>? Responses { get; set; }
        [Required]
		[JsonIgnore]
		public virtual Survey? Survey { get; set; }
        public virtual Recipient? Recipient { get; set; }
	}
}
