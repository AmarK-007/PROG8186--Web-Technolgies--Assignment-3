using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shoecart_ASP_Assignment3.Models {
    public class Product {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public List<string> ImageUrl { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        public decimal ShippingCost { get; set; }

        // Navigation property
        public ICollection<Comment> Comments { get; set; }
    }
    
}
