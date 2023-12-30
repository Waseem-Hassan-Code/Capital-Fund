using Capital.Funds.Models.DTO;

namespace Capital.Funds.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(UserRegistrationDto register);
        Task<LoginResponseDto> Login(LoginDto login);
        Task<string> VerifyEmail(EmailVerificationDto emailVerificationDto);
        Task<string> SendEmailAsync(string email);
        Task<string> UpdatePasswordAsync(string password,string otp);
        Task<string> ResendEmail(string email);
    }
}
