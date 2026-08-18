using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Models
{
    public class Favourite
    {
        [Key]
        public int FavouriteId { get; set; }



        // Customer relationship

        public int CustomerId { get; set; }


        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }





        // Artwork relationship

        public int ArtId { get; set; }


        [ForeignKey("ArtId")]
        public ArtWork? ArtWork { get; set; }

    }
}
