using AVMAPP.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AVMAPP.Data.Infrastructure.AVMDbContext
{
    internal class AVMAppDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
    {
        public AVMAppDbContext(DbContextOptions<AVMAppDbContext> options) : base(options) { }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<ProductImageEntity> ProductImages { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductCommentEntity> ProductComments { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<CartItemEntity> CartItems { get; set; }
        public DbSet<ContactFormEntity> ContactForms { get; set; }

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

            // RoleEntity mapping********************************************************************
            builder.Entity<RoleEntity>().Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            builder.Entity<RoleEntity>().Property(r => r.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");
            //UserEntity mapping***********************************************************************
            builder.Entity<UserEntity>().Property(r => r.Role).HasDefaultValueSql("BUYER");
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
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
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

            // Seed sabit ID'ler
            var roleAdminId = 111;
            var roleSellerId = 222;
            var roleBuyerId = 333;

            var user1Id = 4;
            var user2Id = 5;
            var user3Id = 6;

            // RoleEntity Seed
            builder.Entity<RoleEntity>().HasData(
                new RoleEntity { Id = roleAdminId, Name = "Admin", NormalizedName = "ADMIN", IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new RoleEntity { Id = roleSellerId, Name = "Seller", NormalizedName = "SELLER", IsActive = false, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new RoleEntity { Id = roleBuyerId, Name = "Buyer", NormalizedName = "BUYER", IsActive = false, IsDeleted = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );

            // CategoryEntity Seed
            builder.Entity<CategoryEntity>().HasData(
                new CategoryEntity { Id = 1, Name = "Elektronik" },
                new CategoryEntity { Id = 2, Name = "Kitap" },
                new CategoryEntity { Id = 3, Name = "Giyim" },
                new CategoryEntity { Id = 4, Name = "Ev & Yaşam" },
                new CategoryEntity { Id = 5, Name = "Spor & Outdoor" },
                new CategoryEntity { Id = 6, Name = "Güzellik & Kişisel Bakım" },
                new CategoryEntity { Id = 7, Name = "Oyun & Eğlence" },
                new CategoryEntity { Id = 8, Name = "Müzik & Film" },
                new CategoryEntity { Id = 9, Name = "Ofis & Kırtasiye" },
                new CategoryEntity { Id = 10, Name = "Evcil Hayvan" }
            );

            // UserEntity Seed
            _ = builder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = user1Id,
                    UserName = "testbuyer",
                    NormalizedUserName = "TESTBUYER",
                    Email = "buyer@test.com",
                    NormalizedEmail = "BUYER@TEST.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAE...",
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new UserEntity
                {
                    Id = user2Id,
                    UserName = "testseller",
                    NormalizedUserName = "TESTSELLER",
                    Email = "seller@test.com",
                    NormalizedEmail = "SELLER@TEST.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAE...",
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new UserEntity
                {
                    Id = user3Id,
                    UserName = "testadmin",
                    NormalizedUserName = "TESTADMIN",
                    Email = "admin@test.com",
                    NormalizedEmail = "ADMIN@TEST.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAE...",
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            );

            // ProductEntity Seed (sadece ana bilgiler)
            builder.Entity<ProductEntity>().HasData(
                new ProductEntity { Id = 1, Name = "Akıllı Telefon", Details = "Yeni nesil telefon", Price = 10000, CategoryId = 1, SellerId = user2Id },
                new ProductEntity { Id = 2, Name = "Roman Kitabı", Details = "Popüler roman", Price = 150, CategoryId = 2, SellerId = user2Id },
                new ProductEntity { Id = 3, Name = "Tışört", Details = "Pamuklu tışört", Price = 50, CategoryId = 3, SellerId = user2Id }
            );

            // ProductImageEntity Seed
            builder.Entity<ProductImageEntity>().HasData(
                new ProductImageEntity { Id = 1, ProductId = 1, Url = "https://picsum.photos/200/300" },
                new ProductImageEntity { Id = 2, ProductId = 2, Url = "https://picsum.photos/200/300" },
                new ProductImageEntity { Id = 3, ProductId = 3, Url = "https://picsum.photos/200/300" }
            );

            // ProductCommentEntity Seed
            builder.Entity<ProductCommentEntity>().HasData(
                new ProductCommentEntity { Id = 1, ProductId = 2, UserId = user1Id, Comment = "Bu kitabı çok beğendim!", StarCount = 5, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, IsActive = true, IsDeleted = false },
                new ProductCommentEntity { Id = 2, ProductId = 3, UserId = user1Id, Comment = "Oldukça rahattı", StarCount = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, IsActive = true, IsDeleted = false }
            );



        }
    }
}




