using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Persistence.Main.EntityConfigurations
{
    internal class OrderProductDetailConfiguration : IEntityTypeConfiguration<OrderProductDetail>
    {
        public void Configure(EntityTypeBuilder<OrderProductDetail> builder)
        {
            builder.ToTable("OrderProductDetails");

            builder.Ignore(productDetail => productDetail.TotalPrice);

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

            builder.HasKey(productDetail => productDetail.Id);


            builder.HasOne(productDetail => productDetail.Order).
                WithMany(order => order.ProductDetails).
                HasForeignKey(productDetail => productDetail.OrderId);


            builder.HasOne(productDetail => productDetail.Product).
                WithMany().
                HasForeignKey(productDetail => productDetail.ProductId);

            builder.Property(productDetail => productDetail.ProductId).
                IsRequired();
        }
    }
}
