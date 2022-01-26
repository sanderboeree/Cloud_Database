using OnlineStore.Api.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Api.Infrastructure.EntityFramework.Configurations.Orders
{
    public class OrderShipmentConfig : EntityConfig<OrderShipment>, IEntityTypeConfiguration<OrderShipment>
    {
        public new void Configure(EntityTypeBuilder<OrderShipment> builder)
        {
            base.Configure(builder);
        }
    }
}