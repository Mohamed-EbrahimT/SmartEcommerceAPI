using System;
using System.Collections.Generic;
using FinalProj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartE_Commerce_Data;

namespace FinalProj.Data;

internal partial class ECContext : DbContext
{
    public ECContext()
    {
    }

    public ECContext(DbContextOptions<ECContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.;Database=SmartECommerce;User Id=Mohamed;Password=1234;encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2AFB5998D563");

            entity.Property(e => e.AddressId).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Addresses).HasConstraintName("FK__Address__UserId__4CA06362");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__51BCD7B75FD95DAA");

            entity.Property(e => e.CartId).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Carts).HasConstraintName("FK__Cart__UserId__4D94879B");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B0A30081646");

            entity.Property(e => e.CartItemId).ValueGeneratedNever();

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems).HasConstraintName("FK__CartItem__CartId__4E88ABD4");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems).HasConstraintName("FK__CartItem__Produc__4F7CD00D");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0BE6A49C01");

            entity.Property(e => e.CategoryId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BCF0F94E6D2");

            entity.Property(e => e.OrderId).ValueGeneratedNever();

            entity.HasOne(d => d.Cart).WithMany(p => p.Orders).HasConstraintName("FK__Order__CartId__5070F446");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders).HasConstraintName("FK__Order__OrderStat__5165187F");

            entity.HasOne(d => d.User).WithMany(p => p.Orders).HasConstraintName("FK__Order__UserId__52593CB8");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK__OrderIte__08D097A385FD6372");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Order__534D60F1");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Produ__5441852A");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.OrderStatusId).HasName("PK__OrderSta__BC674CA1A6DC858F");

            entity.Property(e => e.OrderStatusId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A3830A1E484");

            entity.Property(e => e.PaymentId).ValueGeneratedNever();

            entity.HasOne(d => d.Order).WithMany(p => p.Payments).HasConstraintName("FK__Payment__OrderId__5535A963");

            entity.HasOne(d => d.User).WithMany(p => p.Payments).HasConstraintName("FK__Payment__UserId__5629CD9C");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD6B5F7D1C");

            entity.Property(e => e.ProductId).ValueGeneratedNever();

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK__Products__Catego__571DF1D5");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__74BC79CEE20898EC");

            entity.Property(e => e.ReviewId).ValueGeneratedNever();

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews).HasConstraintName("FK__Review__ProductI__5812160E");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews).HasConstraintName("FK__Review__UserId__59063A47");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C71314D94");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__UserRoleId__59FA5E80");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__UserRole__3D978A35800F8C94");

            entity.Property(e => e.UserRoleId).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
