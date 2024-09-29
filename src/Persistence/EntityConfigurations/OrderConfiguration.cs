using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Persistence.Main.Identity;

namespace Store.Persistence.Main.EntityConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o=> o.Id);

            builder.Property(entity => entity.CreatedDate).
                IsRequired().
                HasDefaultValueSql("getdate()");

            builder.Property(entity => entity.CreatedBy).
                IsRequired(false);

            builder.Property(entity => entity.ModifiedDate).
                IsRequired(false);

            builder.Property(entity => entity.ModifiedBy).
                IsRequired(false);

            builder.Property(entity => entity.Version).
                IsRequired(false).
                HasMaxLength(128);

            builder.Property(o => o.IsOrderPayed).
                IsRequired().
                HasDefaultValue(false);

            builder.Property(o => o.Status).
                IsRequired().
                HasDefaultValue(OrderStatus.New);

            builder.HasOne(o => o.OrderAuthor as AppUser).
                WithMany(user => user.Orders).
                HasForeignKey(o=> o.OrderAuthorId);

            builder.Ignore(o => o.TotalPrice);

            builder.HasMany(o => o.Entries).
                WithOne(entry => entry.Order).
                OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.ProductDetails).
                WithOne(productDetail => productDetail.Order).
                OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(order => order.PaymentDetails).
                WithOne(p => p.Order).
                OnDelete(DeleteBehavior.Cascade);

        }
    }
}
