using System.ComponentModel.DataAnnotations;

namespace Shoecart_ASP_Assignment3.Models {
    public class User {
        [Key]
        public int user_id { get; set; } 

        [Required] 
        public string name { get; set; }

        [Required]
        [EmailAddress] 
        public string email { get; set; }

        [Required]
        public string password { get; set; } 

        [Required]
        public string username { get; set; }

        [Required]
        public string purchase_history { get; set; } 

        [Required]
        public string shipping_address { get; set; }
    }
}
