using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerce.TransactionService.Application.Entities;

namespace ECommerce.TransactionService.Infrastructure.Persistence
{
    // Cau hinh Entity cho Payment
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("payments");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.OrderId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.PaymentMethod)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(p => p.TransactionId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PaidAt)
                .IsRequired(false);

            // Index cho TransactionId de tim kiem nhanh
            builder.HasIndex(p => p.TransactionId)
                .IsUnique();
        }
    }
}
