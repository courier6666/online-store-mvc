using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities.Model;

namespace Store.Persistence.Main.EntityConfigurations
{
    public class PaymentDetailConfiguration : IEntityTypeConfiguration<PaymentDetails>
    {
        public void Configure(EntityTypeBuilder<PaymentDetails> builder)
        {
            builder.ToTable("PaymentDetails");
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

            builder.Property(payment => payment.AmountPayed).IsRequired().
                HasColumnType("decimal(12, 2)");


            builder.Property(payment => payment.CashDepositId).
                IsRequired();

            builder.Property(payment => payment.OrderId).IsRequired();
            builder.HasIndex(payment => payment.OrderId).IsUnique();

            builder.HasOne(payment => payment.CashDeposit).WithMany().HasForeignKey(payment => payment.CashDepositId);

            builder.HasOne(payment => payment.Order).
                WithOne(o => o.PaymentDetails).
                HasForeignKey<PaymentDetails>(payment => payment.OrderId);

            builder.HasOne(payment => payment.CashDeposit).
                WithMany(cash => cash.PaymentDetails).
                HasForeignKey(payment => payment.CashDepositId).
                OnDelete(DeleteBehavior.NoAction);

        }
    }
}
