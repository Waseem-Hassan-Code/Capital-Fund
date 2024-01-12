namespace Capital.Funds.Models
{
    public class TenatDetails
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string UserId { get; set; }
        public string PropertyId { get; set; }
        public DateTime MovedIn { get; set; }
        public string MovedOut { get; set;}
        public int RentPerMonth { get; set; }
    }
}
