using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }


        // Customer who owns this cart item
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }



        // Artwork in cart
        public int ArtId { get; set; }

        [ForeignKey("ArtId")]
        public ArtWork? ArtWork { get; set; }

    }
}
