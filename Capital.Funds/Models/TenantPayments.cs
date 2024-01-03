using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Capital.Funds.Models
{
    public class TenantPayments
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TenantId { get; set; }
        public decimal Rent {  get; set; }
        public decimal AreaMaintainienceFee { get; set; }
        public bool isLate { get; set; }
        public decimal LateFee {  get; set; }
        public DateTime RentPayedAt {  get; set; }
        public string Month {  get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public bool isPayable { get; set; } 
    }
}
