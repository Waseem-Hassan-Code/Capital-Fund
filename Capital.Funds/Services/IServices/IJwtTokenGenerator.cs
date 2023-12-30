using Capital.Funds.Models.DTO;

namespace Capital.Funds.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string email,string role);
    }
}
