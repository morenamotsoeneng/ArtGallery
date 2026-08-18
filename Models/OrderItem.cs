using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public Order? Order { get; set; }

        public int ArtId { get; set; }

        public ArtWork? ArtWork { get; set; }

        public decimal Price { get; set; }
    }
}
