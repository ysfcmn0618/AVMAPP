using AVMAPP.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace AVMAPP.Data.Infrastructure.AVMDbContext.Seed
{
    public static class AppDbSeeder
    {
        public static void Seed(ModelBuilder builder)
        {
            // Sabit GUID'ler (ilişkilerde ve tekrar migrationda tutarlılık için)
            var roleAdminId = 1;
            var roleSellerId = 2;
            var roleBuyerId = 3;

            var user1Id = Guid.Parse("a1111111-1111-1111-1111-111111111111");
            var user2Id = Guid.Parse("a2222222-2222-2222-2222-222222222222");
            var user3Id = Guid.Parse("a3333333-3333-3333-3333-333333333333");

            var now = new DateTime(2025, 01, 01);

            // Roller
            builder.Entity<RoleEntity>().HasData(
                new RoleEntity
                {
                    Id = roleAdminId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive = true,
                    IsDeleted = false
                },
                new RoleEntity
                {
                    Id = roleSellerId,
                    Name = "Seller",
                    NormalizedName = "SELLER",
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive = true,
                    IsDeleted = false
                },
                new RoleEntity
                {
                    Id = roleBuyerId,
                    Name = "Buyer",
                    NormalizedName = "BUYER",
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive = true,
                    IsDeleted = false
                }
            );

            // Kullanıcılar
            var hasher = new PasswordHasher<UserEntity>();
            builder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = user1Id,
                    UserName = "testadmin",
                    Email = "admin@test.com",
                    EmailConfirmed = true,
                    PasswordHash = "$2b$12$UWvRj8WHnPh7nXe/cx4gkeNI0DNbGWmG3arPbGBUyQQa9R5hRkZ4.", // Admin123!
                    RoleId = roleAdminId,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive = true,
                    IsDeleted = false,
                    NormalizedUserName = "TESTADMIN",
                    NormalizedEmail = "ADMIN@TEST.COM",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new UserEntity
                {
                    Id = user2Id,
                    UserName = "testseller",
                    Email = "seller@test.com",
                    EmailConfirmed = true,
                    PasswordHash = "$2b$12$xO/srWkQzcaTVNcRHcKAjeee/4KC75cR9fI7Ihw5.JQe9Jl/GziAW", // Seller123!
                    RoleId = roleSellerId,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive = true,
                    IsDeleted = false,
                    NormalizedUserName = "TESTSELLER",
                    NormalizedEmail = "SELLER@TEST.COM",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new UserEntity
                {
                    Id = user3Id,
                    UserName = "testbuyer",
                    Email = "buyer@test.com",
                    EmailConfirmed = true,
                    PasswordHash = "$2b$12$BWogJtVAAIijaMAFiVuD6eEPfanzBhrlV5NnQdAqA18VZz6F2zF8q", // Buyer123!
                    RoleId = roleBuyerId,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive = true,
                    IsDeleted = false,
                    NormalizedUserName = "TESTBUYER",
                    NormalizedEmail = "BUYER@TEST.COM",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            );


            // Kategoriler
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

            // Ürünler (SellerId: user2Id, CategoryId: int)
            builder.Entity<ProductEntity>().HasData(
                new ProductEntity
                {
                    Id = 1,
                    Name = "Akıllı Telefon",
                    Description = "Yeni nesil telefon",
                    Price = 10000,
                    CategoryId = 1,
                    SellerId = user2Id,
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive = true,
                    IsDeleted = false,
                    StockAmount = 100
                },
                new ProductEntity
                {
                    Id = 2,
                    Name = "Roman Kitabı",
                    Description = "Popüler roman",
                    Price = 150,
                    CategoryId = 2,
                    SellerId = user2Id,
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive = true,
                    IsDeleted = false,
                    StockAmount = 200
                },
                new ProductEntity
                {
                    Id = 3,
                    Name = "Tişört",
                    Description = "Pamuklu tişört",
                    Price = 50,
                    CategoryId = 3,
                    SellerId = user2Id,
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive = true,
                    IsDeleted = false,
                    StockAmount = 245
                }
            );

            // Ürün Resimleri
            builder.Entity<ProductImageEntity>().HasData(
                new ProductImageEntity { Id = 1, ProductId = 1, Url = "https://picsum.photos/200/300" },
                new ProductImageEntity { Id = 2, ProductId = 2, Url = "https://picsum.photos/200/300" },
                new ProductImageEntity { Id = 3, ProductId = 3, Url = "https://picsum.photos/200/300" }
            );

            // Ürün Yorumları (UserId: user1Id, ProductId: int)
            builder.Entity<ProductCommentEntity>().HasData(
                new ProductCommentEntity
                {
                    Id = 1,
                    ProductId = 2,
                    UserId = user1Id,
                    Comment = "Bu kitabı çok beğendim!",
                    StarCount = 5,
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive = true,
                    IsDeleted = false,
                    IsConfirmed = true
                },
                new ProductCommentEntity
                {
                    Id = 2,
                    ProductId = 3,
                    UserId = user1Id,
                    Comment = "Oldukça rahattı",
                    StarCount = 4,
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive = true,
                    IsDeleted = false,
                    IsConfirmed = true
                }
            );
        }
    }
}
