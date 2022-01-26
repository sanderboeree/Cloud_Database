using System;
using System.Collections.Generic;
using OnlineStore.Api.Domain.Orders;

namespace OnlineStore.Api.Application.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordCurrent { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<string> Roles { get; set; } = new List<string>();

        public UserDto() { }

        public UserDto(User entity)
        {
            Id = entity.Id;
            Email = entity.Email;
            Name = entity.Name;
            PhoneNumber = entity.PhoneNumber;

            if (entity.UserRoles != null)
            {
                foreach (var userRole in entity.UserRoles)
                {
                    Roles.Add(userRole.Role.Name);
                }
            }
        }

        public User ToEntity(User entity = null)
        {
            entity ??= new User();

            entity.Id = Id;
            entity.Name = Name;
            entity.Email = Email;
            entity.UserName = Email;
            entity.PhoneNumber = PhoneNumber;

            return entity;
        }
    }
}
