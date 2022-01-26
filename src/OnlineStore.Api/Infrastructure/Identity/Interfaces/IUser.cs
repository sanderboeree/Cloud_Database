using System;

namespace OnlineStore.Api.Infrastructure.Identity.Interfaces
{
    public interface IUser
    {
        public Guid? Id { get; }

        bool IsInRole(string role);
    }
}
