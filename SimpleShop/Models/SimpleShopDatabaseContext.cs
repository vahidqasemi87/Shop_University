using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SimpleShop.Models
{
    public partial class SimpleShopDatabaseContext : DbContext
    {
        public SimpleShopDatabaseContext(DbContextOptions<SimpleShopDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductAttribute> ProductAttribute { get; set; }
        public virtual DbSet<ProductAttributeValue> ProductAttributeValue { get; set; }
        public virtual DbSet<ProductPhoto> ProductPhoto { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<Subcategory> Subcategory { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Category__737584F6D36FE8BB")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhotoFilename).HasMaxLength(50);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("FK__City__StateId__2B3F6F97");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Username)
                    .HasName("UQ__Customer__536C85E4AB1FC9E4")
                    .IsUnique();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Family)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK__Customer__CityId__2F10007B");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Order__CustomerI__47DBAE45");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__OrderDeta__Order__4D94879B");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__OrderDeta__Produ__4E88ABD4");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PhotoFilename).HasMaxLength(50);

                entity.HasOne(d => d.Subcategory)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.SubcategoryId)
                    .HasConstraintName("FK__Product__Subcate__37A5467C");
            });

            modelBuilder.Entity<ProductAttribute>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.UnitId })
                    .HasName("UQ__ProductA__373ADA3D2426DF5E")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.ProductAttribute)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK__ProductAt__UnitI__412EB0B6");
            });

            modelBuilder.Entity<ProductAttributeValue>(entity =>
            {
                entity.HasOne(d => d.ProductAttribute)
                    .WithMany(p => p.ProductAttributeValue)
                    .HasForeignKey(d => d.ProductAttributeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductAt__Produ__44FF419A");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductAttributeValue)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductAt__Produ__440B1D61");
            });

            modelBuilder.Entity<ProductPhoto>(entity =>
            {
                entity.Property(e => e.PhotoFilename)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPhoto)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductPh__Produ__3A81B327");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__State__737584F666F4EAEC")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Subcategory>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhotoFilename).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Subcategory)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Subcatego__Categ__34C8D9D1");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Unit__737584F682726953")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username)
                    .HasName("UQ__User__536C85E4333CEC4D")
                    .IsUnique();

                entity.Property(e => e.Family)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
