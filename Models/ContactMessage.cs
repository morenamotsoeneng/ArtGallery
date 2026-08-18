using System;
using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    public class ContactMessage
    {
        [Key]
        public int ContactMessageId { get; set; }

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        [Required]
        [StringLength(100)]
        public string Subject { get; set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; set; }

        public DateTime DateSent { get; set; } = DateTime.Now;

        // Admin reply
        public string? AdminReply { get; set; }

        public DateTime? ReplyDate { get; set; }

        public bool IsReplied { get; set; } = false;
    }
}
