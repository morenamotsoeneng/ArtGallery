using System;
using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int ArtId { get; set; }
        public ArtWork? ArtWork { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(500)]
        public string Comment { get; set; }

        public DateTime DatePosted { get; set; } = DateTime.Now;
    }
}
