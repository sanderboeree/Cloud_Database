using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Threading;
using System.Threading.Tasks;
using OnlineStore.Api.Domain.Orders;
using OnlineStore.Api.Infrastructure.EntityFramework.Configurations.Users;
using OnlineStore.Api.Infrastructure.EntityFramework.Data;
using OnlineStore.Api.Infrastructure.EntityFramework.SqlServer;
using OnlineStore.Api.Infrastructure.EntityFramework.Configurations.Orders;

namespace OnlineStore.Api.Infrastructure.EntityFramework
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<OrderShipment> OrderShipments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .ReplaceService<IRelationalAnnotationProvider, OnlineStoreSqlServerAnnotationProvider>()
                .ReplaceService<IMigrationsSqlGenerator, OnlineStoreSqlServerMigrationsSqlGenerator>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RefreshTokenConfig());
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new RoleConfig());
            builder.ApplyConfiguration(new OrderConfig());
            builder.ApplyConfiguration(new ProductConfig());
            builder.ApplyConfiguration(new OrderProductConfig());
            builder.ApplyConfiguration(new AddressConfig());
            builder.ApplyConfiguration(new OrderShipmentConfig());


            RoleData.Seed(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
