namespace Identity.Microservice.Core.Dto.Identity
{
    public class PasswordResetDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}