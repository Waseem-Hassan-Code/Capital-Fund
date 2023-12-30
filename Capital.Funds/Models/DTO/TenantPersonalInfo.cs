namespace Capital.Funds.Models.DTO
{
    public class TenantPersonalInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public bool isEmailVerified { get; set; }
    }
}
