using OnlineStore.Api.Application.Users.Interfaces;
using OnlineStore.Api.Domain.Orders;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace OnlineStore.Api.Application.Users
{
    public class UserActivator : IUserActivator
    {
        private readonly UserManager<User> _userManager;

        public UserActivator(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GetAccountActivationToken(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(Guid userid, string token)
        {
            var user = await _userManager.FindByIdAsync(userid.ToString());
            return _userManager.ConfirmEmailAsync(user, token).Result;
        }
    }
}
