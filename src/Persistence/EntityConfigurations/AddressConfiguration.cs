﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities.Model;
using Store.Persistence.Main.Identity;

namespace Store.Persistence.Main.EntityConfigurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(address => address.Id);

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

            builder.HasOne(address => address.Person as AppUser).WithOne(user => user.Address)
                .HasForeignKey<Address>(address => address.PersonId).
                OnDelete(DeleteBehavior.SetNull);
        }
    }
}