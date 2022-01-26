using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Application.Users.Interfaces
{
    public interface ITokenService
    {
        Task<TokenDto> GenerateTokenAsync(CreateTokenDto model, CancellationToken cancellationToken = default);

        bool ValidateToken(ValidateTokenDto model, out ClaimsPrincipal claimsPrincipal);
    }
}