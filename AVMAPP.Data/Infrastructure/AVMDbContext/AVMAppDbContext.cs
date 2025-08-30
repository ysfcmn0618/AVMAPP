using AVMAPP.Data.Entities;
using AVMAPP.Data.Infrastructure.AVMDbContext.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


namespace AVMAPP.Data.Infrastructure.AVMDbContext
{
    public class AVMAppDbContext : DbContext
    {
        public AVMAppDbContext(DbContextOptions<AVMAppDbContext> options) : base(options) { }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        public DbSet<ProductImageEntity> ProductImages { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductCommentEntity> ProductComments { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<CartItemEntity> CartItems { get; set; }
        public DbSet<ContactFormEntity> ContactForms { get; set; }
        public DbSet<DiscountEntity> Discounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<UserEntity>().ToTable("Users");
            builder.Entity<RoleEntity>().ToTable("Roles");
            builder.Entity<ProductImageEntity>().ToTable("ProductImages");
            builder.Entity<ProductEntity>().ToTable("Products");
            builder.Entity<ProductCommentEntity>().ToTable("ProductComments");
            builder.Entity<OrderItemEntity>().ToTable("OrderItems");
            builder.Entity<OrderEntity>().ToTable("Orders");
            builder.Entity<CategoryEntity>().ToTable("Categories");
            builder.Entity<CartItemEntity>().ToTable("CartItems");
            builder.Entity<ContactFormEntity>().ToTable("ContactForms");
            builder.Entity<DiscountEntity>().ToTable("Discounts");

            // RoleEntity mapping********************************************************************
            builder.Entity<RoleEntity>().Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            builder.Entity<RoleEntity>().Property(r => r.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");
            //UserEntity mapping***********************************************************************
            //builder.Entity<UserEntity>().Property(r => r.Role.Name).HasDefaultValue("BUYER");
            builder.Entity<UserEntity>()
          .HasIndex(u => u.Email)
          .IsUnique();
            //USerROleEntity mapping***************************************************************
            builder.Entity<UserRoleEntity>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.Entity<UserRoleEntity>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserRoleEntity>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            //Tüm kullanıcılar (rolü ne olursa olsun) ürün sahibi olabilir gibi gösterir, ama bu sadece fiziksel veri modeli için geçerli.Sadece satıcıların ürünleri olabilir kontrolunu servislerde tanımlayacağız!!!
            builder.Entity<UserEntity>()
             .HasMany(u => u.Products)
             .WithOne(p => p.Seller)
             .HasForeignKey(p => p.SellerId)
             .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //kullanıcnın birden fazla siparişi olur her siparişin bir sahibi olur 
            builder.Entity<UserEntity>()
                .HasMany(u => u.Orders)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //Her yorum bir sahibi olur bir kullanıcın çok yorumu olabilir
            builder.Entity<UserEntity>().HasMany(c => c.Comments)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired(false);
            //ProductEntity mapping ************************************************************
            //bir ürünün çok resmi bir resmin bir ürünü olablir
            builder.Entity<ProductEntity>()
                .HasMany(p => p.Images)
                .WithOne()
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ProductEntity>()
        .Property(p => p.Price)
        .HasColumnType("decimal(18,2)");
            builder.Entity<ProductEntity>()
                .HasOne(o=>o.Discount)
                .WithOne()
                .HasForeignKey<ProductEntity>(p=>p.DiscountId)
                .OnDelete(DeleteBehavior.SetNull);
            //bir ürünün çok yorumu bir yorum bir ürününün olabilir
            builder.Entity<ProductEntity>()
                .HasMany(c => c.Comments)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
            //bir ürünün bir kategorisi olmak zorunda bir kategorinin çok ürünü olabilir ve zorunlu alan 
            builder.Entity<ProductEntity>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);
            //Order Entitiy mapping ------------------------------------------------------------
            builder.Entity<OrderEntity>()
                .HasMany(o => o.OrderItems)
                .WithOne()
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);
            builder.Entity<OrderEntity>()
                .HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<OrderItemEntity>()
       .Property(p => p.UnitPrice)
       .HasColumnType("decimal(18,2)");
            //Discount Entity mapping --------------------------------------------------------
            
            AppDbSeeder.Seed(builder);
        }
    }
}




