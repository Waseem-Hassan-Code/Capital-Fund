using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Capital.Funds.Models
{
    public class PropertyDetails
    {
        [Key]
        public string Id { get; set; }
        public string PropertyName { get; set; }
        public string Address { get; set; }
        public string TypeofProperty { get; set; }
        public string NumberofBedrooms { get; set; }
        public string NumberofBathrooms { get; set; }
        public bool isAvailable { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }

    }
}
