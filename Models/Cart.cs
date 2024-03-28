using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Shoecart_ASP_Assignment3.Models {
    public class Cart {
        [Key]
        public int CartId { get; set; }

        [Required]
        public int UserId { get; set; }

        // Navigation property
        [JsonIgnore]
        public ICollection<CartItem> CartItems { get; set; }
    }

    public class CartItem {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        [ForeignKey("Cart")]
        public int CartId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Navigation property
        [JsonIgnore]
        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
