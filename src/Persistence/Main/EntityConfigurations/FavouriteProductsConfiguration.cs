using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities.Model;
using Store.Persistence.Main.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.Main.EntityConfigurations
{
    public class FavouriteProductsConfiguration : IEntityTypeConfiguration<FavouriteProduct>
    {
        public void Configure(EntityTypeBuilder<FavouriteProduct> builder)
        {
            builder.ToTable("FavoruiteProducts");
            builder.HasKey(f => new {f.UserId, f.ProductId});

            builder.HasOne(f => f.User as AppUser).
                WithMany().
                HasForeignKey(f => f.UserId).
                IsRequired();

            builder.HasOne(f => f.Product).
                WithMany().
                HasForeignKey(f => f.ProductId).
                IsRequired();
        }
    }
}
