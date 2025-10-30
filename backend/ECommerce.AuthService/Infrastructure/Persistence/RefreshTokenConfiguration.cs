using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerce.AuthService.Application.Entities;

namespace ECommerce.AuthService.Infrastructure.Persistence
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refreshTokens");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).IsRequired();
            builder.Property(r => r.UserId).IsRequired();
            builder.Property(r => r.Token).IsRequired();
            builder.Property(r => r.ExpiredAt).IsRequired();

            builder.HasIndex(r => r.UserId);
            builder.HasOne(r => r.User)
                   .WithMany()
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
