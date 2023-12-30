using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Capital.Funds.Utils
{
    public static class SD
    {
        public static string User = "user";
        public static string Admin = "admin";
        public static string IncorrectPassword = "Incorrect-Password";
        public static string UserNotFound = "Not Found.";
        public static string DatabaseName = "CaptialFund_TenantsRecord.db";
        public static string AlreadyRegistered = "User with this email is already registered.";
        public static string RegistrationSuccess = "User Registered Successfully.";
        public static string RegistrationFailed = "Registration Failed.";
        public static string InvalidEmailAddress = "This email address is not valid.";
        public static string EmailVerified = "Email Verified Successfully.";
        public static string IncorrectOtp = "Incorrect OTP.";
        public static string Unverified = "We are unable to verify your email at the moment, try again later.";
        public static string RecordUpdated = "Record updated successfully.";
        public static string RecordNotUpdated = "Failed to update record.";


        private const int OtpLength = 6;
        private static readonly Random random = new Random();
        public static string GenerateOtp()
        {
            int minValue = (int)Math.Pow(10, OtpLength - 1);
            int maxValue = (int)Math.Pow(10, OtpLength);
            int otp = random.Next(minValue, maxValue);
            return otp.ToString($"D{OtpLength}");
        }




        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
        public static string HashPassword(string password, string salt)
        {
            int iterations = 10000;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), iterations))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
