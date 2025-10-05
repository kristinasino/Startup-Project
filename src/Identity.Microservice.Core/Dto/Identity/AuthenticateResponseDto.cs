using Core.Dto.User;
namespace Core.Dto
{
    public class AuthenticateResponseDto
    {
        public UserLoginDto User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public AuthenticateResponseDto(UserLoginDto user, string accessToken, string refreshToken)
        {
            User = user;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}