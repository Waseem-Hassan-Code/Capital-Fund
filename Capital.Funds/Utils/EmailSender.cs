using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Capital.Funds.Utils
{
    public class EmailSender
    {
        public async Task<(bool, string)> SendEmailAsync(string email, string subject, string? message="")
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("capitalfundllc@gmail.com", "alsd bmrm mlcx fmgo")
                };

                string OTP = SD.GenerateOtp();

                await client.SendMailAsync(new MailMessage
                {
                    From = new MailAddress("capitalfundllc@gmail.com"),
                    To = { new MailAddress(email) },
                    Subject = subject,
                    Body = GetEmailBody(OTP),
                    IsBodyHtml = true
                });

                return (true, OTP);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return (false,""); 
            }
        }



        private string GetEmailBody(string otpCode) {

            return $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Capital Fund - OTP Verification</title>
    <style>
        body {{
            font-family: 'Arial', sans-serif;
            background-color: #f4f4f4;
            color: #333;
            margin: 0;
            padding: 0;
        }}

        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }}

        .header {{
            text-align: center;
            margin-bottom: 20px;
        }}

        .header h1 {{
            color: #3498db;
        }}

        .content {{
            font-size: 16px;
            line-height: 1.6;
        }}

        .otp-code {{
            font-size: 24px;
            font-weight: bold;
            color: #e74c3c;
            margin-top: 10px;
        }}

        .footer {{
            margin-top: 20px;
            text-align: center;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>Capital Fund</h1>
        </div>

        <div class=""content"">
            <p>Dear user,</p>
            <p>Thank you for choosing Capital Fund. To verify your account, please use the following OTP code:</p>
            <p class=""otp-code"">{otpCode}</p>
            <p>This OTP is valid for a limited time. Do not share it with anyone for security reasons.</p>
        </div>

        <div class=""footer"">
            <p>Best regards,<br>Capital Fund Team</p>
        </div>
    </div>
</body>
</html>
";
        
        }
    }
}
