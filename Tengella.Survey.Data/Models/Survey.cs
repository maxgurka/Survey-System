using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tengella.Survey.Data.Models

{
    public class Survey
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ICollection<Question>? Questions { get; set; }
		public virtual ICollection<Respondent>? Respondents { get; set; }
	}
}
