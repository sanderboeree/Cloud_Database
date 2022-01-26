using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using OnlineStore.Api.Application.Users.Interfaces;
using OnlineStore.Api.Domain.Orders;
using OnlineStore.Api.Infrastructure.Configuration;
using OnlineStore.Api.Infrastructure.Repositories.Interfaces;
using OnlineStore.Api.Infrastructure.Specifications;

namespace OnlineStore.Api.Application.Users
{
    public class UserAuthenticator : IUserAuthenticator
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly TokenSettings _jwtBearerTokenSettings;
        private readonly JwtSecurityTokenHandler _handler;
        private readonly ITokenService _tokenService;

        public UserAuthenticator(
            UserManager<User> userManager,
            TokenValidationParameters tokenValidationParameters,
            IRepository<RefreshToken> refreshTokenRepository,
            IOptions<TokenSettings> jwtTokenOptions, ITokenService tokenService)
        {
            _userManager = userManager;
            _jwtBearerTokenSettings = jwtTokenOptions.Value;
            _tokenValidationParameters = tokenValidationParameters;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _handler = new JwtSecurityTokenHandler();
        }

        public async Task<User> AuthenticateAsync(UserCredentialDto userCredentialDto)
        {
            if (userCredentialDto == null)
            {
                throw new ArgumentNullException(nameof(userCredentialDto));
            }

            var user = await _userManager.Users/*.Include(u => u.OnlineStoreUser)*/.FirstOrDefaultAsync(u => u.NormalizedUserName == userCredentialDto.Email.ToUpper());
            if (user == null)
            {
                throw new KeyNotFoundException($"No user with name '{userCredentialDto.Email}'");
            }

            var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, userCredentialDto.Password);

            return result == PasswordVerificationResult.Failed ? null : user;
        }

        public async Task<TokenDto> RefreshTokenAsync(string token, Guid refreshtoken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            var validatedToken = GetPrincipalFromToken(token);
            if (validatedToken == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                throw new SecurityTokenException("Token not expired");
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _refreshTokenRepository.FindOneAsync(new WithId<RefreshToken>(refreshtoken), cancellationToken);

            if (storedRefreshToken == null ||
                DateTime.UtcNow > storedRefreshToken.Expires ||
                storedRefreshToken.Invalidated ||
                storedRefreshToken.Used ||
                storedRefreshToken.JwtId != jti)
            {
                throw new SecurityTokenException("Invalid refresh token");

            }
            storedRefreshToken.Used = true;
            await _refreshTokenRepository.SaveAsync(storedRefreshToken, cancellationToken);
            var id = Guid.Parse(validatedToken.Claims.Single(x => x.Type == TokenService.ClaimTypeId).Value);
            var user = await _userManager.Users./*Include(u => u.OnlineStoreUser).*/FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            return await GenerateTokenAsync(user, cancellationToken);
        }

        public async Task<TokenDto> GenerateTokenAsync(User user, CancellationToken cancellationToken = default)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var model = new CreateTokenDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Name = user.Name,
                Email = user.Email,
                GenerateRefreshToken = true
            };

            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                model.Claims.Add(new Claim("roles", role.ToLower()));
            }

            return await _tokenService.GenerateTokenAsync(model, cancellationToken);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = _handler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                return !IsJwtWithValidSecurityAlgorithm(validatedToken) ? null : principal;
            }
            catch
            {
                return null;
            }
        }

        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken securityToken)
        {
            return securityToken is JwtSecurityToken jwtSecurityToken && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
