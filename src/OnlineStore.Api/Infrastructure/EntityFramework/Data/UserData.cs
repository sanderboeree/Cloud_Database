using Microsoft.AspNetCore.Identity;
using OnlineStore.Api.Domain.Orders;

namespace OnlineStore.Api.Infrastructure.EntityFramework.Data
{
    public class UserData
    {
        public static void Seed(UserManager<User> userManager)
        {
            if (userManager.FindByEmailAsync("admin@OnlineStore.nl").Result == null)
            {
                var user = new User
                {
                    UserName = "admin@OnlineStore.nl",
                    Email = "admin@OnlineStore.nl",
                    Name = "Administrator"
                };

                var result = userManager.CreateAsync(user, "Test@2021").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, RoleData.Admin).Wait();
                }
            }
        }
    }
}
