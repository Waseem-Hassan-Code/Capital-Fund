using Capital.Funds.Models.DTO;

namespace Capital.Funds.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string name,string role, string id);
    }
}
