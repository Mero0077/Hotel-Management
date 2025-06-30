namespace Hotel_Management.Services.IServices
{
    public interface IEmailService
    {
        public Task SendAsync(string to,string subject,string body);
        public Task SendOTP(string to,string otp);
    }
}
