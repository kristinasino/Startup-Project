namespace Identity.Microservice.Core.Dto.Identity;

public class ChangePasswordDto
{
    public string oldPassword { get; set; }
    public string newPassword { get; set; }
    public string confirmPassword { get; set; }
}