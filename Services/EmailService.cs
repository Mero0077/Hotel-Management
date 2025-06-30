using Hotel_Management.Services.IServices;
using System.Net;
using System.Net.Mail;

namespace Hotel_Management.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration) 
        {
            this._configuration = configuration;
        }
        public async Task SendAsync(string to, string subject, string body)
        {
            var host = _configuration["EmailSettings:Host"];
            var port = int.Parse(_configuration["EmailSettings:Port"]!);
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderPassword = _configuration["EmailSettings:SenderPassword"];

            using var SmtpClient = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };
            var message = new MailMessage(senderEmail!, to,subject,body);
            await SmtpClient.SendMailAsync(message);
        }

        public async Task SendOTP(string to, string otp)
        {
            string subject = "Your OTP Code ";
            string body = $"Your OTP Code is :{otp}";
            await SendAsync(to, subject, body);
        }
    }
}
