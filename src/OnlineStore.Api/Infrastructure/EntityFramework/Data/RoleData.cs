using Microsoft.EntityFrameworkCore;
using System;
using OnlineStore.Api.Domain.Orders;

namespace OnlineStore.Api.Infrastructure.EntityFramework.Data
{
    public class RoleData
    {
        public const string Admin = "admin";
        public const string Customer = "customer";


        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = new Guid("62b56f46-85ba-4712-b37d-e4f01852606e"), ConcurrencyStamp = "d784d793-596c-45d7-aa09-878b63707a31", Name = Admin, NormalizedName = Admin.ToUpper() },
                new Role { Id = new Guid("f934ed38-0dc3-4c41-adf3-e19d6a886d8f"), ConcurrencyStamp = "e9276ef0-cc63-4fe8-aa31-8bf6b88f127d", Name = Customer, NormalizedName = Customer.ToUpper() }
            );
        }
    }
}
