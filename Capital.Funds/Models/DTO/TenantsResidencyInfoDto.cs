namespace Capital.Funds.Models.DTO
{
    public class TenantsResidencyInfoDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }    
        public string PropertyName { get; set; }
        public DateTime MovedIn {  get; set; }
        public string MovedOut { get; set; }
        public int RentPerMonth { get; set; }
    }
}
