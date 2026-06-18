using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SmileService.Models;

public partial class SmileServiceDBContext : DbContext
{
    public SmileServiceDBContext()
    {
    }

    public SmileServiceDBContext(DbContextOptions<SmileServiceDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderPart> OrderParts { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=SmileServiceDB_Final;Integrated Security=True;Connect Timeout=30;Encrypt=False";

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.Property(e => e.DeviceType).HasMaxLength(50);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.SerialNumber).HasMaxLength(100);

            entity.HasOne(d => d.Client).WithMany(p => p.Devices)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Devices_Clients");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.DeviceId, "UQ_Orders_Device").IsUnique();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FinishedDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Device).WithOne(p => p.Order)
                .HasForeignKey<Order>(d => d.DeviceId)
                .HasConstraintName("FK_Orders_Devices");

            entity.HasOne(d => d.Master).WithMany(p => p.Orders)
                .HasForeignKey(d => d.MasterId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<OrderPart>(entity =>
        {
            entity.Property(e => e.WriteOffDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Master).WithMany(p => p.OrderParts)
                .HasForeignKey(d => d.MasterId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_OrderParts_Users");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderParts)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderParts_Orders");

            entity.HasOne(d => d.Part).WithMany(p => p.OrderParts)
                .HasForeignKey(d => d.PartId)
                .HasConstraintName("FK_OrderParts_Parts");
        });

        modelBuilder.Entity<Part>(entity =>
        {
            entity.Property(e => e.PartName).HasMaxLength(200);
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SellingPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .HasColumnName("SKU");
            entity.Property(e => e.Supplier).HasMaxLength(100);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Method).HasMaxLength(50);
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Payments_Orders");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Login, "UQ_Users_Login").IsUnique();

            entity.Property(e => e.ContactInfo).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
