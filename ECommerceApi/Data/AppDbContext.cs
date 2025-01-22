using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Account>()
            .HasOne(a => a.Role) 
            .WithMany(r => r.Accounts) 
            .HasForeignKey(a => a.RoleId) 
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Account>()
            .HasIndex(a => a.Email)
            .IsUnique();

        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.Account)
            .WithMany(a => a.RefreshTokens)
            .HasForeignKey(rt => rt.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
