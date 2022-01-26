using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace OnlineStore.Api.Application.Users.Interfaces
{
    public interface IUserActivator
    {
        Task<string> GetAccountActivationToken(Guid userId);
        Task<IdentityResult> ConfirmEmailAsync(Guid userId, string token);
    }
}
