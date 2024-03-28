using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
// other using statements

namespace Shoecart_ASP_Assignment3.Models {
    public class Order {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        // Navigation property
        [JsonIgnore]
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
        [JsonIgnore]
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}