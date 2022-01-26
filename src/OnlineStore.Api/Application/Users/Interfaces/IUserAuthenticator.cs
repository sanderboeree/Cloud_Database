using OnlineStore.Api.Domain.Orders;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineStore.Api.Application.Users.Interfaces
{
    public interface IUserAuthenticator
    {
        Task<User> AuthenticateAsync(UserCredentialDto userCredentialDto);
        Task<TokenDto> GenerateTokenAsync(User identityUser, CancellationToken cancellationToken = default);
        Task<TokenDto> RefreshTokenAsync(string token, Guid refreshtoken, CancellationToken cancellationToken = default);
    }
}
