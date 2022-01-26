using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Api.Domain.Orders;

namespace OnlineStore.Api.Infrastructure.EntityFramework.Configurations.Orders
{
    public class OrderProductConfig : EntityConfig<OrderProduct>, IEntityTypeConfiguration<OrderProduct>
    {
        public new void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            base.Configure(builder);

            builder.HasOne(entity => entity.Product)
                .WithMany()
                .HasForeignKey(entity => entity.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(entity => entity.Order)
                .WithMany()
                .HasForeignKey(entity => entity.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}