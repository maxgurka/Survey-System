using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tengella.Survey.Data.Models

{
    public class Survey
    {
        public Survey()
        {
            //OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        public int Id { get; set; }
        [BindProperty]
        public string Name { get; set; }
        public ICollection<Question> Questions{ get; set; }
    }
}
