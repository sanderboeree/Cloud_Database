using System;
using System.Linq;
using System.Security.Claims;
using OnlineStore.Api.Application.Users;
using OnlineStore.Api.Infrastructure.EntityFramework.Data;

namespace OnlineStore.Api.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetId(this ClaimsPrincipal user)
        {
            return Guid.TryParse(user.FindFirst(TokenService.ClaimTypeId)?.Value, out var userId) ? userId : Guid.Empty;
        }

        public static bool IsCoach(this ClaimsPrincipal user)
        {
            return user.HasClaim(RoleData.Customer);
        }

        public static bool IsManager(this ClaimsPrincipal user)
        {
            return user.HasClaim(RoleData.Customer);
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.HasClaim(RoleData.Admin);
        }

        public static bool HasClaim(this ClaimsPrincipal user, string role)
        {
            if (user.HasClaim(claim => claim.Type == ClaimTypes.Role))
            {
                var roleClaims = user.Claims.Where(a => a.Type == ClaimTypes.Role);

                return roleClaims.Any(claim => claim.Value.Equals(role, StringComparison.InvariantCultureIgnoreCase));
            }
            return false;
        }
    }
}