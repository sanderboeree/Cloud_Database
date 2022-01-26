using OnlineStore.Api.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Api.Infrastructure.EntityFramework.Configurations.Orders
{
    public class AddressConfig : EntityConfig<Address>, IEntityTypeConfiguration<Address>
    {
        public new void Configure(EntityTypeBuilder<Address> builder)
        {
            base.Configure(builder);

            builder.HasOne(entity => entity.User)
                .WithMany()
                .HasForeignKey(entity => entity.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}