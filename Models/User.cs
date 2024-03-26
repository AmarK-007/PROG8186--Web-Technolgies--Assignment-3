using System.ComponentModel.DataAnnotations;

namespace Shoecart_ASP_Assignment3.Models {
    public class User {
        [Key]
        public int user_id { get; set; }
        public required string name { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
        public required string username { get; set; }
        public required string purchase_history { get; set; }
        public required string shipping_address { get; set; }
    }
}
