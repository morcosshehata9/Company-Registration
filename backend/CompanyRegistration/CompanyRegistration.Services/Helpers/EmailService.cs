using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRegistration.Services
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendOtpAsync(string toEmail, string otpCode)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            using (var client = new SmtpClient(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"])))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);

                var mail = new MailMessage();
                mail.From = new MailAddress(emailSettings["SenderEmail"], emailSettings["SenderName"]);
                mail.To.Add(toEmail);
                mail.Subject = "Your OTP Code";
                mail.Body = $"""
                            Hello,

                            Your OTP code is: {otpCode}

                            Thanks,
                            {emailSettings["SenderName"]}
                            """;
                mail.IsBodyHtml = false;

                await client.SendMailAsync(mail);
            }
        }
    }
}
