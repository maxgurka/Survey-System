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

        public string Name { get; set; }

        // Navigation Property
        //public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
