using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ArtGallery.ViewModels
{
    public class ArtworkViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(50)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Artist name is required")]
        [StringLength(100)]
        public string ArtistName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 1000000)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Artwork status is required")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "Please select an image")]
        public IFormFile ImageFile { get; set; }
    }
}