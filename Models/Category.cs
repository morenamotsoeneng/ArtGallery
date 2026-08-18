using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50)]
        public string CategoryName { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        // Navigation Property
        public ICollection<ArtWork>? ArtWorks { get; set; }
    }
}