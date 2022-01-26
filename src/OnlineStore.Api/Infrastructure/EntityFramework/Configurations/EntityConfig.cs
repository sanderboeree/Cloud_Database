
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Api.Domain;
using OnlineStore.Api.Infrastructure.EntityFramework.SqlServer;

namespace OnlineStore.Api.Infrastructure.EntityFramework.Configurations
{
    public abstract class EntityConfig<TEntity> where TEntity : Entity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder
                .HasKey(entity => entity.Id);

            builder
                .Property(entity => entity.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(entity => entity.Modified)
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()")
                .HasAnnotation(OnlineStoreAnnotation.ModifiedTrigger, true)
                .ValueGeneratedOnAddOrUpdate()
                .IsRequired();

            builder.Property(entity => entity.Created)
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();
        }
    }
}
