using Microsoft.EntityFrameworkCore;
using MGM.GoodHamburger.Domain.Entities;

namespace MGM.GoodHamburger.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Subtotal).HasPrecision(10, 2);
            entity.Property(e => e.DiscountPercentage).HasPrecision(5, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(10, 2);
            entity.Property(e => e.Total).HasPrecision(10, 2);

            entity.HasOne(e => e.Sandwich)
                .WithMany()
                .HasForeignKey(e => e.SandwichId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.SideDish)
                .WithMany()
                .HasForeignKey(e => e.SideDishId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Drink)
                .WithMany()
                .HasForeignKey(e => e.DrinkId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.Property(e => e.Type).IsRequired();
        });

        // Seed inicial do cardápio
        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { Id = 1, Name = "X Burger", Price = 5.00m, Type = MenuItemType.Sandwich },
            new MenuItem { Id = 2, Name = "X Egg", Price = 4.50m, Type = MenuItemType.Sandwich },
            new MenuItem { Id = 3, Name = "X Bacon", Price = 7.00m, Type = MenuItemType.Sandwich },
            new MenuItem { Id = 4, Name = "Batata frita", Price = 2.00m, Type = MenuItemType.SideDish },
            new MenuItem { Id = 5, Name = "Refrigerante", Price = 2.50m, Type = MenuItemType.Drink }
        );
    }
}