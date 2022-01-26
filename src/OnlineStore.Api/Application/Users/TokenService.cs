using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OnlineStore.Api.Application.Users.Interfaces;
using OnlineStore.Api.Domain.Orders;
using OnlineStore.Api.Infrastructure.Configuration;
using OnlineStore.Api.Infrastructure.Repositories.Interfaces;

namespace OnlineStore.Api.Application.Users
{
    public class TokenService : ITokenService
    {
        public const string ClaimTypeId = "id";

        public const string InviteSubject = "invite";

        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly TokenSettings _jwtBearerTokenSettings;
        private readonly JwtSecurityTokenHandler _handler;

        public TokenService(
            IRepository<RefreshToken> refreshTokenRepository,
            IOptions<TokenSettings> jwtTokenOptions)
        {
            _jwtBearerTokenSettings = jwtTokenOptions.Value;
            _refreshTokenRepository = refreshTokenRepository;
            _handler = new JwtSecurityTokenHandler();
        }

        public async Task<TokenDto> GenerateTokenAsync(CreateTokenDto model, CancellationToken cancellationToken = default)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var claims = GetDefaultClaims(model);

            if (model.Claims?.Count > 0)
            {
                claims.AddRange(model.Claims);
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtBearerTokenSettings.SecretKey));

            claims.Add(!string.IsNullOrWhiteSpace(model.Subject)
                ? new Claim(JwtRegisteredClaimNames.Sub, model.Subject)
                : new Claim(JwtRegisteredClaimNames.Sub, model.UserName));

            var expiresIn = DateTime.Now.Add(_jwtBearerTokenSettings.Expires);

            if (model.ExpiresIn.HasValue)
            {
                expiresIn = DateTime.Now.Add(model.ExpiresIn.Value);
            }

            var token = new JwtSecurityToken(
                issuer: _jwtBearerTokenSettings.Issuer,
                audience: _jwtBearerTokenSettings.Audience,
                claims: claims,
                expires: expiresIn,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            if (model.GenerateRefreshToken)
            {
                var refreshToken = new RefreshToken
                {
                    JwtId = token.Id,
                    UserId = model.UserId,
                    Expires = DateTime.UtcNow.AddMonths(6)
                };

                await _refreshTokenRepository.SaveAsync(refreshToken, cancellationToken);

                return new TokenDto
                {
                    Token = _handler.WriteToken(token),
                    TokenValidTo = token.ValidTo,
                    RefreshToken = refreshToken.Id,
                    RefreshTokenValidTo = refreshToken.Expires
                };
            }

            return new TokenDto
            {
                Token = _handler.WriteToken(token),
                TokenValidTo = token.ValidTo
            };
        }

        private static List<Claim> GetDefaultClaims(CreateTokenDto model)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, model.Name),
                new Claim(ClaimTypeId, model.UserId.ToString())
            };
            return claims;
        }

        public bool ValidateToken(ValidateTokenDto model, out ClaimsPrincipal claimsPrincipal)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtBearerTokenSettings.SecretKey));

                claimsPrincipal = tokenHandler.ValidateToken(model.Token, new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidIssuer = _jwtBearerTokenSettings.Issuer,
                    ValidAudience = _jwtBearerTokenSettings.Audience,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true
                }, out var outToken);

                if (outToken != null && outToken.ValidTo > DateTime.UtcNow)
                {
                    if (model.ValidateSubject && claimsPrincipal.HasClaim(claim => claim.Type == ClaimTypes.NameIdentifier))
                    {
                        if (model.Subject == claimsPrincipal.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value)
                        {
                            return true;
                        }
                        claimsPrincipal = null;
                        return false;
                    }
                    if (model.ValidateSubject && !claimsPrincipal.HasClaim(claim => claim.Type == ClaimTypes.NameIdentifier))
                    {
                        claimsPrincipal = null;
                        return false;
                    }
                    return true;
                }

                claimsPrincipal = null;
                return false;
            }
            catch (Exception)
            {
                claimsPrincipal = null;
                return false;
            }
        }
    }
}