namespace Capital.Funds.Models.DTO
{
    public class ComplaintsDTO
    {
        public int ComplaintId { get; set; }
        public string TenantName { get; set; }
        public string ComplaintTitle { get; set; }
        public string ComplaintDetails { get; set; }
        public bool IsFixed { get; set; }
        public DateTime ComplainDate { get; set; }
    }
}
