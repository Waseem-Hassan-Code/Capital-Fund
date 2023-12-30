using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Capital.Funds.Models
{
    public class Users
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password {  get; set; }

        [JsonIgnore]
        public string Salt { get; set; }
        public int Gender { get; set; }
        public string Role { get; set; } 
        public bool IsActive { get; set; }
        public string OTP { get; set; }
        public bool isEmailVerified { get; set; }
    }
}
