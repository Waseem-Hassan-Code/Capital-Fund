using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Capital.Funds.Models
{
    public class TenantComplaints
    {
        [Key]
        public string Id {  get; set; }
        public string TenantId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public bool IsFixed { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime UpdatedAt { get; set;}
    }
}
