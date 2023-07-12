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

        [Required]
        public string Name { get; set; } = string.Empty;
        public string info { get; set; } = string.Empty;

        // Navigation Property
        //public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
