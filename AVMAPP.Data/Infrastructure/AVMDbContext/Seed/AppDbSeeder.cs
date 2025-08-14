using AVMAPP.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Infrastructure.AVMDbContext.Seed
{
    public static class AppDbSeeder
    {
        public static void Seed(ModelBuilder builder)
        {
            var roleAdminId = Guid.Parse("e1f49a71-5580-4d0f-b31f-a3552df64a92");
            var roleSellerId = Guid.Parse("c5e8a91e-bcc9-4e0c-a602-b66e4217766e");
            var roleBuyerId = Guid.Parse("d0ad2064-227a-4a60-b3ea-503b6cf3c407");

            var user1Id = Guid.NewGuid();
            var user2Id = Guid.NewGuid();
            var user3Id = Guid.NewGuid();

            var now = new DateTime(2025, 01, 01);

            builder.Entity<RoleEntity>().HasData(
                new RoleEntity { Id = roleAdminId, Name = "Admin", NormalizedName = "ADMIN" },
                new RoleEntity { Id = roleSellerId, Name = "Seller", NormalizedName = "SELLER" },
                new RoleEntity { Id = roleBuyerId, Name = "Buyer", NormalizedName = "BUYER" }
            );

            var hasher = new PasswordHasher<UserEntity>();
            builder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = user1Id,
                    Email = "admin@test.com",
                    UserName = "testadmin",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin123!"),
                    RoleId = roleAdminId,
                    SecurityStamp = "seed-stamp-admin",
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive = true,
                    IsDeleted = false
                },
                new UserEntity
                {
                    Id = user2Id,
                    Email = "seller@test.com",
                    UserName = "testseller",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Seller123!"),
                    RoleId = roleSellerId,
                    SecurityStamp = "seed-stamp-seller",
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive = true,
                    IsDeleted = false
                },
                new UserEntity
                {
                    Id = user3Id,
                    Email = "buyer@test.com",
                    UserName = "testbuyer",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Buyer123!"),
                    RoleId = roleBuyerId,
                    SecurityStamp = "seed-stamp-buyer",
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive = true,
                    IsDeleted = false
                }
            );

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

            builder.Entity<ProductEntity>().HasData(
                new ProductEntity { Id = 1, Name = "Akıllı Telefon", Details = "Yeni nesil telefon", Price = 10000, CategoryId = 1, SellerId = user2Id },
                new ProductEntity { Id = 2, Name = "Roman Kitabı", Details = "Popüler roman", Price = 150, CategoryId = 2, SellerId = user2Id },
                new ProductEntity { Id = 3, Name = "Tişört", Details = "Pamuklu tişört", Price = 50, CategoryId = 3, SellerId = user2Id }
            );

            builder.Entity<ProductImageEntity>().HasData(
                new ProductImageEntity { Id = 1, ProductId = 1, Url = "https://picsum.photos/200/300" },
                new ProductImageEntity { Id = 2, ProductId = 2, Url = "https://picsum.photos/200/300" },
                new ProductImageEntity { Id = 3, ProductId = 3, Url = "https://picsum.photos/200/300" }
            );

            builder.Entity<ProductCommentEntity>().HasData(
                new ProductCommentEntity { Id = 1, ProductId = 2, UserId = user1Id, Comment = "Bu kitabı çok beğendim!", StarCount = 5, CreatedAt = now, UpdatedAt = now, IsActive = true, IsDeleted = false },
                new ProductCommentEntity { Id = 2, ProductId = 3, UserId = user1Id, Comment = "Oldukça rahattı", StarCount = 4, CreatedAt = now, UpdatedAt = now, IsActive = true, IsDeleted = false }
            );
        }
    }
}
