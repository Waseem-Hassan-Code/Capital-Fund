namespace Capital.Funds.Models.DTO
{
    public class TenantPersonalInfoDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public bool isEmailVerified { get; set; }
    }
}
