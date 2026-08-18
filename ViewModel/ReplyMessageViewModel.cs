using System.ComponentModel.DataAnnotations;

namespace ArtGallery.ViewModels
{
    public class ReplyMessageViewModel
    {
        public int ContactMessageId { get; set; }

        public string CustomerName { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        [Required(ErrorMessage = "Reply is required")]
        [StringLength(1000)]
        public string AdminReply { get; set; }
    }
}
