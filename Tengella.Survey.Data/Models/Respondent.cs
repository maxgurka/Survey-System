using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengella.Survey.Data.Models
{
    public class Respondent
    {
        [Key]
        public int Id { get; set; }
		public required ICollection<Response> Responses { get; set; }
	}
}
