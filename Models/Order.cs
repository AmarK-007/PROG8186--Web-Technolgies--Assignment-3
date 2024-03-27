using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shoecart_ASP_Assignment3.Models {
    public class Order {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        // Navigation property
        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem {
        [Key]
        public int OrderItemId { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
