using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tengella.Survey.Data.Models

{
    public class Survey
    {
        [Key]
        public int Id { get; set; }
        [BindProperty]
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required ICollection<Question> Questions { get; set; }
    }
}
