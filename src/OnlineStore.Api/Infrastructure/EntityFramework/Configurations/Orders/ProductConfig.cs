using OnlineStore.Api.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Api.Infrastructure.EntityFramework.Configurations.Orders
{
    public class ProductConfig : EntityConfig<Product>, IEntityTypeConfiguration<Product>
    {
        public new void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);
        }
    }
}