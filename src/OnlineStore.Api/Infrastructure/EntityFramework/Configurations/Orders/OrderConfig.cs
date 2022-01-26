using OnlineStore.Api.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Api.Infrastructure.EntityFramework.Configurations.Orders
{
    public class OrderConfig : EntityConfig<Order>, IEntityTypeConfiguration<Order>
    {
        public new void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.HasOne(entity => entity.BillingAddress)
                .WithMany()
                .HasForeignKey(entity => entity.BillingAddressId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(entity => entity.ShippingAddress)
                .WithMany()
                .HasForeignKey(entity => entity.ShippingAddressId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(entity => entity.User)
                .WithMany()
                .HasForeignKey(entity => entity.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
