using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Store.Persistence.Main.Identity;

namespace Store.Persistence.Main.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            public DateOnlyConverter() :
                base(dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
                    dateTime => new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day))
            {

            }
        }
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(user => user.Id);

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

            builder.Ignore(u => u.Roles);

            builder.Property(user => user.FirstName).
                HasMaxLength(128).
                IsRequired();

            builder.Property(user => user.LastName).
                HasMaxLength(128).
                IsRequired();

            builder.Property(user => user.Birthday).
                HasConversion<DateOnlyConverter>().
                HasColumnType("date").
                IsRequired();


            builder.Property(user => user.Email).
                HasMaxLength(128).
                IsRequired();

            builder.HasIndex(user => user.Email).IsUnique();

            builder.Property(user => user.Login).
                HasMaxLength(16).
                IsRequired();

            builder.HasIndex(user => user.Login).IsUnique();

            builder.Property(user => user.PasswordHash).
                IsRequired().
                HasMaxLength(128);

            builder.HasMany(user => user.Orders).
                WithOne(order => order.OrderAuthor as AppUser);

            builder.HasMany(user => user.CashDeposits).
                WithOne(cashDeposit => cashDeposit.User as AppUser);

            builder.HasOne(user => user.Address).
                WithOne(address => address.Person as AppUser).
                OnDelete(DeleteBehavior.Cascade);
        }
    }
}
