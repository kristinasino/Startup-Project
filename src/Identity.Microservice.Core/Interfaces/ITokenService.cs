using System.Threading.Tasks;
using Core.Dto;
using Core.Dto.Identity;
using Domain.Entities.Identity;
using Identity.Microservice.Domain.Entities;
using UserModule.Domain.Entities;

namespace Web.IdentityFactory
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
        Task<RefreshToken> GenerateRefreshToken(int userId);
        Task<AuthenticateResponseDto> RefreshToken(RequestRefreshTokenDto requestRefreshTokenDto);

        Task RemoveRefreshTokenAsync(string refreshToken);
        Task<string> GenerateReportsToken(int userId);
    }
}