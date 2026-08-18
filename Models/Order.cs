using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Completed";

        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
