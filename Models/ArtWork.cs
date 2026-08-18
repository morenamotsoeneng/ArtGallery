using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    public class ArtWork
    {
        [Key]
        public int ArtId { get; set; }

        [Required(ErrorMessage ="Title is required")]
        [StringLength(50,MinimumLength =3,ErrorMessage ="Title must be between 3 and 50 characters")]
        public string Title { get; set;}
        //[Required(ErrorMessage = "Status is required")]
        public bool Status { get; set; }
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(1, 1000000)]
      
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Artist name is required")]
        [StringLength(100, ErrorMessage = "Artist name cannot exceed 100 characters")]
        public string ArtistName { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Select an image")]
        public string? ImageURL { get; set;}
        public ICollection<Favourite>? Favourites { get; set; }

    }
}
