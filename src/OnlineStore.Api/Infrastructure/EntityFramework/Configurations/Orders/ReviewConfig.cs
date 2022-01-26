using OnlineStore.Api.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Api.Infrastructure.EntityFramework.Configurations.Orders
{
    public class ReviewConfig : EntityConfig<Review>, IEntityTypeConfiguration<Review>
    {
        public new void Configure(EntityTypeBuilder<Review> builder)
        {
            base.Configure(builder);

            builder.HasOne(entity => entity.Product)
                .WithMany()
                .HasForeignKey(entity => entity.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(entity => entity.User)
                .WithMany()
                .HasForeignKey(entity => entity.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}