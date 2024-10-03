using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities.Interfaces;
using Store.Domain.Entities.Model;
using Store.Persistence.Main.Identity;

namespace Store.Persistence.Main.EntityConfigurations
{
    public class CashDepositConfiguration : IEntityTypeConfiguration<CashDeposit>
    {
        public void Configure(EntityTypeBuilder<CashDeposit> builder)
        {
            builder.ToTable("CashDeposits");

            builder.HasKey(cashDeposit => cashDeposit.Id);

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

            builder.Property(cashDeposit => cashDeposit.CurrentMoneyBalance).
                HasColumnType("decimal(12, 2)").
                HasDefaultValue(0.00m).
                IsRequired();

            builder.HasDiscriminator<string>("DepositType").
                HasValue<AdministratorCashDeposit>("admin").
                HasValue<UserCashDeposit>("user");

            builder.Property(cashDeposit => cashDeposit.UserId).
                IsRequired();

            builder.HasOne(cashDeposit => cashDeposit.User as AppUser).
                WithMany(user => user.CashDeposits).
                HasForeignKey(cashDeposit => cashDeposit.UserId);

        }
    }
}
