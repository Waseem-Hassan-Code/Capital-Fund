using Capital.Funds.Database;
using Capital.Funds.Models;
using Capital.Funds.Models.DTO;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;

namespace Capital.Funds.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDb _db;
        private readonly IJwtTokenGenerator _token;

        public AuthService(ApplicationDb db,IJwtTokenGenerator token)
        {
            _db = db;
            _token = token;
        }

        public async Task<LoginResponseDto> Login(LoginDto login)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == login.Email.ToLower());
                if (user != null)
                {
                    string storedSalt = user.Salt;
                    string hashedPassword = SD.HashPassword(login.Password, storedSalt);

                    if (user.Password == hashedPassword)
                    {
                        UserDto userInfo = new UserDto
                        {
                            Name = user.Name,
                            Email = user.Email,
                            Gender = user.Gender,
                        };

                        string token = _token.GenerateToken(login.Email, user.Role);

                        LoginResponseDto response = new LoginResponseDto
                        {
                            User = userInfo,
                            Token = token
                        };

                        return response;
                    }
                    else
                    {
                        return new LoginResponseDto
                        {
                            User = null,
                            Token = SD.IncorrectPassword
                        };
                    }
                }
                else
                {
                    return new LoginResponseDto
                    {
                        User = null,
                        Token = SD.UserNotFound
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return null;
        }


        public async Task<string> Register(UserRegistrationDto register)
        {
            try {
                var checkUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == register.Email);
                if (checkUser != null)
                {
                    return SD.AlreadyRegistered;
                }

                string Salt = SD.GenerateSalt();
                string HashedPassword = SD.HashPassword(register.Password,Salt);

                EmailSender emailSender = new();
                var email = await emailSender.SendEmailAsync(register.Email, "Email Verification");

                if (email.Item1 == false)
                    return SD.InvalidEmailAddress;

                Users user = new Users
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = register.Name,
                    Email = register.Email.ToLower(),
                    Password = HashedPassword,
                    Salt = Salt,
                    Gender = register.Gender,
                    Role = SD.User,
                    IsActive = false,
                    OTP = email.Item2,
                    isEmailVerified = false,
                };

                await _db.Users.AddAsync(user);
                int result = await _db.SaveChangesAsync();

                if (result > 0)
                    return SD.RegistrationSuccess;

                return SD.RegistrationFailed;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message, ex);
            }
            return null;
        }

        public async Task<string> ResendEmail(string email)
        {
            try
            {
                var checkUser = await _db.Users.FirstOrDefaultAsync(e => e.Email == email.ToLower() && e.OTP != null);
                if (checkUser != null)
                {
                    EmailSender emailSender = new();
                    var emailAsync = await emailSender.SendEmailAsync(email, "Email Verification");

                    if (emailAsync.Item1 == false)
                        return SD.InvalidEmailAddress;

                    checkUser.isEmailVerified = false;
                        checkUser.OTP = emailAsync.Item2;
                        await _db.SaveChangesAsync();
                        return SD.EmailVerified;
                }
                return SD.Unverified;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<string> SendEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdatePasswordAsync(string password)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdatePasswordAsync(string password, string otp)
        {
            throw new NotImplementedException();
        }

        public async Task<string> VerifyEmail(EmailVerificationDto emailVerificationDto)
        {
            try
            {
                var checkUser = await _db.Users.FirstOrDefaultAsync(e => e.Email ==emailVerificationDto.Email.ToLower() && e.OTP != null);
                if (checkUser != null)
                {
                    if(checkUser.OTP == emailVerificationDto.Otp)
                    {
                        checkUser.isEmailVerified = true;
                        checkUser.OTP = null;
                        await _db.SaveChangesAsync();
                        return SD.EmailVerified;
                    }
                    return SD.IncorrectOtp;
                }
                return SD.Unverified;
            }
            catch (Exception ex) {
                return null;
            }
        }
    }
}
