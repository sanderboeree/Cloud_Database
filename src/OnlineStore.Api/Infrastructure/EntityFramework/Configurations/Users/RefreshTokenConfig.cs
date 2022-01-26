using OnlineStore.Api.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineStore.Api.Infrastructure.EntityFramework.Configurations.Users
{
    public class RefreshTokenConfig : EntityConfig<RefreshToken>, IEntityTypeConfiguration<RefreshToken>
    {
        public new void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            base.Configure(builder);

            builder
                .Property(entity => entity.JwtId)
                .IsRequired();

            builder.Property(entity => entity.Expires)
                .HasColumnType("DATETIME")
                .IsRequired();

            builder
                .Property(entity => entity.Used)
                .IsRequired();

            builder
                .Property(entity => entity.UserId)
                .IsRequired();

            builder
                .Property(entity => entity.Invalidated)
                .IsRequired();

            builder
                .HasOne(entity => entity.User)
                .WithMany()
                .HasForeignKey(entity => entity.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
