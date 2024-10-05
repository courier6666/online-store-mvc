using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Persistence.Main.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(entity => entity.CreatedDate).
                IsRequired().
                HasDefaultValueSql("getdate()");

            builder.Property(entity => entity.CreatedBy).
                IsRequired(false).
                HasMaxLength(128);

            builder.Property(entity => entity.ModifiedDate).
                IsRequired(false);

            builder.Property(entity => entity.ModifiedBy).
                IsRequired(false).
                HasMaxLength(128);

            builder.Property(entity => entity.Version).
                IsRequired(false).
                HasMaxLength(128);

            builder.Property(p => p.Name).HasMaxLength(128);
            builder.HasIndex(p => p.Name).IsUnique();

            builder.Property(p => p.Category).HasMaxLength(128);

            builder.Property(p => p.Description).HasMaxLength(512);

            builder.Property(p => p.Price).
                HasDefaultValue(0.00m).
                HasColumnType("decimal(12,2)").
                IsRequired();
        }
    }
}
