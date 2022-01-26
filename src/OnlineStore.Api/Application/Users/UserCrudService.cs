using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnlineStore.Api.Application.Users.Interfaces;
using OnlineStore.Api.Domain.Orders;
using OnlineStore.Api.Infrastructure.EntityFramework;

namespace OnlineStore.Api.Application.Users
{
    public class UserCrudService : IUserCrudService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public UserCrudService(UserManager<User> userManager, RoleManager<Role> roleManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task<UserDto> GetByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            var user = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || user.IsDeleted)
            {
                throw new KeyNotFoundException(nameof(userId));
            }
            return new UserDto(user);
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.IsDeleted)
            {
                throw new KeyNotFoundException(nameof(email));
            }
            return new UserDto(user);
        }

        public async Task<IReadOnlyCollection<UserDto>> GetAsync(CancellationToken cancellationToken = default)
        {
            var users = await _userManager.Users.ToListAsync(cancellationToken);
            return users.Select(user => new UserDto(user)).OrderBy(a => a.Name).ToList().AsReadOnly();
        }

        public async Task<UserDto> CreateAsync(UserDto userDto, ICollection<string> roles)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            var user = userDto.ToEntity();

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                throw CreateInvalidOperationException(result);
            }

            result = await _userManager.AddToRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                throw CreateInvalidOperationException(result);
            }

            return new UserDto(user);
        }

        public async Task<UserDto> UpdateAsync(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            var user = await _userManager.FindByIdAsync(userDto.Id.ToString());
            userDto.ToEntity(user);

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                var resultPassRemove = await _userManager.RemovePasswordAsync(user);

                if (!resultPassRemove.Succeeded)
                {
                    throw CreateInvalidOperationException(resultPassRemove);
                }

                var resultPassAdd = await _userManager.AddPasswordAsync(user, userDto.Password);

                if (!resultPassAdd.Succeeded)
                {
                    throw CreateInvalidOperationException(resultPassRemove);
                }
            }

            var result = await _userManager.UpdateAsync(user);
            userDto.PasswordCurrent = null;

            return result.Succeeded
                ? userDto
                : throw CreateInvalidOperationException(result);
        }

        public async Task<IdentityResult> ChangePassword(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            var user = await _userManager.FindByIdAsync(userDto.Id.ToString());
            userDto.ToEntity(user);

            return await _userManager.ChangePasswordAsync(user, userDto.PasswordCurrent, userDto.Password);
        }

        public async Task<UserDto> AddRoleAsync(Guid userId, string role)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                throw new ArgumentNullException(nameof(role));
            }

            var roleEntity = await _roleManager.FindByNameAsync(role);

            if (roleEntity == null)
            {
                throw new KeyNotFoundException(nameof(role));
            }

            var user = await _userManager.Users.Where(user => user.Id == userId && !user.IsDeleted)
                .Include(user => user.UserRoles)
                .ThenInclude(user => user.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new KeyNotFoundException(nameof(userId));
            }

            if (user.UserRoles.All(r => r.Role.Id != roleEntity.Id))
            {
                await _dbContext.Database.ExecuteSqlRawAsync("INSERT INTO AspNetUserRoles (UserId,RoleId) VALUES ({0},{1})", user.Id, roleEntity.Id);
            }

            return await GetByIdAsync(userId);
        }

        public async Task<UserDto> RemoveRoleAsync(Guid userId, string role)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                throw new ArgumentNullException(nameof(role));
            }

            var roleEntity = await _roleManager.FindByNameAsync(role);

            if (roleEntity == null)
            {
                throw new KeyNotFoundException(nameof(role));
            }

            var user = await _userManager.Users.Where(user => user.Id == userId && !user.IsDeleted)
                .Include(user => user.UserRoles)
                .ThenInclude(user => user.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new KeyNotFoundException(nameof(userId));
            }

            if (user.UserRoles.Any(r => r.Role.Id == roleEntity.Id))
            {
                await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM AspNetUserRoles WHERE UserId = {0} AND RoleId = {1}", user.Id, roleEntity.Id);
            }

            return await GetByIdAsync(userId);
        }

        public async Task DeleteAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new KeyNotFoundException(nameof(userId));
            }

            user.Email = $"{user.Id}+{user.Email}";
            user.UserName = $"{user.Id}+{user.UserName}";
            user.IsDeleted = true;

            await _userManager.UpdateAsync(user);
        }

        public async Task<bool> ExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var user = await _userManager.FindByEmailAsync(email);
            return user != null && !user.IsDeleted;
        }

        private static InvalidOperationException CreateInvalidOperationException(IdentityResult result)
        {
            return new InvalidOperationException(result.Errors.Select(error => $"{error.Code}: {error.Description}").Aggregate((current, next) => $"{current}, {next}"));
        }
    }
}
