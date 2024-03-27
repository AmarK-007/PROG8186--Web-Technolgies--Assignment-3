using System.ComponentModel.DataAnnotations;

namespace Shoecart_ASP_Assignment3.Models {
    public class Comment {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int Rating { get; set; }

        public string Image { get; set; }

        [Required]
        public string Text { get; set; }

        // Navigation properties
        public Product Product { get; set; }
        public User User { get; set; }
    }
}
