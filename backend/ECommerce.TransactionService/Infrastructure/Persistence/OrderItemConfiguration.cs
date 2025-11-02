using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerce.TransactionService.Application.Entities;

namespace ECommerce.TransactionService.Infrastructure.Persistence
{
    // Cau hinh Entity cho OrderItem
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("orderItems");
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Id)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(oi => oi.OrderId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(oi => oi.ProductId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(oi => oi.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.Quantity)
                .IsRequired()
                .HasDefaultValue(1);
        }
    }
}
