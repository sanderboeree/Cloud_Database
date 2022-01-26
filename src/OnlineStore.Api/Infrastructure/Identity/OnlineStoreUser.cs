using Microsoft.AspNetCore.Http;
using System;
using OnlineStore.Api.Infrastructure.Extensions;
using OnlineStore.Api.Infrastructure.Identity.Interfaces;

namespace OnlineStore.Api.Infrastructure.Identity
{
    public class OnlineStoreUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public OnlineStoreUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Guid? Id
        {
            get
            {
                return _accessor.HttpContext?.User.GetId();
            }
        }

        public bool IsInRole(string role) => _accessor.HttpContext?.User?.HasClaim(role) ?? false;

    }
}
