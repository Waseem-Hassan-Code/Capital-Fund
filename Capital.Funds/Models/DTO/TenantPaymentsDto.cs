namespace Capital.Funds.Models.DTO
{
    public class TenantPaymentsDto
    {
        public string Id { get; set; }
        public string TenantName { get; set; }
        public decimal Rent { get; set; }
        public decimal AreaMaintainienceFee { get; set; }
        public bool isLate { get; set; }
        public decimal LateFee { get; set; }
        public DateTime RentPayedAt { get; set; }
        public string Month { get; set; }
        public bool isPayable { get; set; }
    }
}
