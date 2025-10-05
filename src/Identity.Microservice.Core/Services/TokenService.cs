using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dto;
using Core.Dto.Identity;
using Core.Dto.User;
using Domain.Entities.Identity;
using Identity.Microservice.Domain.Entities;
using Infrastructure.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Common.Exceptions;
using Shared.Entities.Constants;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Domain.Entities;
using Web.IdentityFactory;

namespace Identity.Microservice.Core.Services
{
    public class TokenService : BaseService, ITokenService
    {
        private readonly TokenSettings TokenSettings;

        public TokenService(IUnitOfWork unitOfWork, IOptions<TokenSettings> tokenSettings, IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper,httpContextAccessor)
        {
            TokenSettings = tokenSettings.Value;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(TokenSettings.Key);
            var claim = new ClaimsIdentity(new Claim[] { });

            var permission = user.UserRoles.SelectMany(x => x.Role.RolePermissions
                .Select(y => y.Permission));

            {
                var claims = permission
                    .Select(item => new Claim(GlobalConstants.AppAccess, item.Code));

                claim.AddClaims(claims);
                claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                claim.AddClaim(new Claim(ClaimTypes.Role, user.UserRoles.First().Role.Name));
                claim.AddClaim(new Claim("tenant", user.TenantId.HasValue ? user.TenantId.ToString() : ""));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = TokenSettings.Aud,
                Issuer = TokenSettings.Iss,
                Subject = claim,
                Expires = DateTime.Now.AddMinutes(TokenSettings.TokenTimeoutMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<RefreshToken> GenerateRefreshToken(int userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = generateRefreshToken(),
                Expires = DateTime.Now.AddDays(TokenSettings.RefreshTokenTimeoutDays),
                UserId = userId
            };

            await UnitOfWork.GetRepository<RefreshToken>().AddAsync(refreshToken);
            await UnitOfWork.CommitAsync();

            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience =
                    false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenSettings.Key)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

            return principal;
        }

        public async Task<AuthenticateResponseDto> RefreshToken(RequestRefreshTokenDto requestRefreshTokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(requestRefreshTokenDto.Token);
            var userId = int.Parse(principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value); //this is mapped to the Name claim by default
            var user = await UnitOfWork.GetRepository<User>().AsTableNoTracking
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .ThenInclude(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new UnauthorizedException("Not authorized");

            var refreshTokenEntity = await UnitOfWork.GetRepository<RefreshToken>()
                .AsTableNoTracking
                .FirstOrDefaultAsync(x => x.Token == requestRefreshTokenDto.RefreshToken && x.Expires > DateTimeOffset.Now);

            if (refreshTokenEntity == null || refreshTokenEntity.UserId != user.Id)
                throw new UnauthorizedException("Not authorized");

            await using var transaction = await UnitOfWork.BeginTransactionAsync();
            
            //Remove old refresh token it's better for security issues to refresh the refresh token together with new token
            await RemoveRefreshTokenAsync(refreshTokenEntity.Token);

            var newRefreshToken = await GenerateRefreshToken(user.Id);
            var newToken = GenerateJwtToken(user);

            await transaction.CommitAsync();
            
            return new AuthenticateResponseDto(Mapper.Map<UserLoginDto>(user), newToken, newRefreshToken.Token);
        }

        public async Task RemoveRefreshTokenAsync(string refreshToken)
        {
            var existingToken = await UnitOfWork.GetRepository<RefreshToken>()
                .AsTrackingEntity
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (existingToken != null)
                UnitOfWork.GetRepository<RefreshToken>().HardDelete(existingToken);

            await UnitOfWork.CommitAsync();
        }
        
        public async Task<string> GenerateReportsToken(int userId)
        {
            var user = await UnitOfWork.GetRepository<UserToken>().AsTableNoTracking
                .FirstOrDefaultAsync(x => x.UserId == userId
                                          && x.ExpiryDate > DateTime.Now);

            if (user != null)
                return user.Token;
            
            var newToken = generateRefreshToken();
                
            var newUserToken = new UserToken()
            {
                Token = newToken,
                UserId = userId,
                ExpiryDate = DateTime.Now.AddMinutes(TokenSettings.ReportTokenTimeout),
            };
            
            await UnitOfWork.GetRepository<UserToken>().AddAsync(newUserToken);

            await UnitOfWork.CommitAsync();
            
            return newToken;
        }
        
        private string generateRefreshToken()
        {
            using var rngCryptoServiceProvider = RandomNumberGenerator.Create();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}