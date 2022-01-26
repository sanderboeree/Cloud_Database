using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStore.Api.Application.Users.Interfaces
{
    public interface IUserCrudService
    {
        Task<UserDto> GetByIdAsync(Guid userId);
        Task<UserDto> GetByEmailAsync(string email);
        Task<UserDto> CreateAsync(UserDto userDto, ICollection<string> roles);
        Task<UserDto> UpdateAsync(UserDto userDto);
        Task<IdentityResult> ChangePassword(UserDto userDto);
        Task<UserDto> AddRoleAsync(Guid userId, string role);
        Task<UserDto> RemoveRoleAsync(Guid userId, string role);
        Task DeleteAsync(Guid userId);
        Task<bool> ExistsAsync(string email);
    }
}
