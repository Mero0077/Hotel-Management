namespace Hotel_Management.DTOs.Account
{
    public class ResetPasswordRequestDto
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
    }
}
