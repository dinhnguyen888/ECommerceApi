using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      
        modelBuilder.Entity<Account>()
            .HasOne(a => a.Role) 
            .WithMany(r => r.Accounts) 
            .HasForeignKey(a => a.RoleId) 
            .OnDelete(DeleteBehavior.SetNull); 

        base.OnModelCreating(modelBuilder);
    }
}
