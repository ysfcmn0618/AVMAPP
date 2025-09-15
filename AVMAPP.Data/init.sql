IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE TABLE [CartItems] (
        [Id] int NOT NULL IDENTITY,
        [UserId] uniqueidentifier NOT NULL,
        [ProductId] int NOT NULL,
        [Quantity] tinyint NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_CartItems] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE TABLE [Categories] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Color] nvarchar(max) NOT NULL,
        [Icon] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE TABLE [ContactForms] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Message] nvarchar(max) NOT NULL,
        [SeenAt] datetime2 NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_ContactForms] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE TABLE [Discounts] (
        [Id] int NOT NULL IDENTITY,
        [DiscountRate] tinyint NOT NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NOT NULL,
        [Enabled] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Discounts] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE TABLE [Roles] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
        [UpdatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [NormalizedName] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] uniqueidentifier NOT NULL,
        [ConcurrencyStamp] nvarchar(max) NOT NULL,
        [NormalizedUserName] nvarchar(max) NOT NULL,
        [UserName] nvarchar(max) NOT NULL,
        [Email] nvarchar(450) NOT NULL,
        [NormalizedEmail] nvarchar(max) NOT NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [FirstName] nvarchar(max) NULL,
        [LastName] nvarchar(max) NULL,
        [Address] nvarchar(max) NULL,
        [AddressOther] nvarchar(max) NULL,
        [Password] nvarchar(max) NULL,
        [ResetPasswordToken] nvarchar(max) NULL,
        [RoleId] int NOT NULL,
        [FullName] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [RefreshToken] nvarchar(max) NULL,
        [RefreshTokenExpiryTime] datetime2 NULL,
        [SecurityStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE TABLE [Orders] (
        [Id] int NOT NULL IDENTITY,
        [UserId] uniqueidentifier NOT NULL,
        [OrderCode] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [UserEntityId] uniqueidentifier NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Orders_Users_UserEntityId] FOREIGN KEY ([UserEntityId]) REFERENCES [Users] ([Id]),
        CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE TABLE [Products] (
        [Id] int NOT NULL IDENTITY,
        [SellerId] uniqueidentifier NOT NULL,
        [CategoryId] int NOT NULL,
        [DiscountId] int NULL,
        [Name] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [StockAmount] tinyint NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Products_Discounts_DiscountId] FOREIGN KEY ([DiscountId]) REFERENCES [Discounts] ([Id]) ON DELETE SET NULL,
        CONSTRAINT [FK_Products_Users_SellerId] FOREIGN KEY ([SellerId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE TABLE [UserRoles] (
        [UserId] uniqueidentifier NOT NULL,
        [RoleId] int NOT NULL,
        CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE TABLE [OrderItems] (
        [Id] int NOT NULL IDENTITY,
        [OrderId] int NOT NULL,
        [ProductId] int NOT NULL,
        [Quantity] tinyint NOT NULL,
        [UnitPrice] decimal(18,2) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE TABLE [ProductComments] (
        [Id] int NOT NULL IDENTITY,
        [ProductId] int NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Comment] nvarchar(max) NULL,
        [StarCount] tinyint NOT NULL,
        [IsConfirmed] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_ProductComments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProductComments_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ProductComments_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE TABLE [ProductImages] (
        [Id] int NOT NULL IDENTITY,
        [ProductId] int NOT NULL,
        [Url] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_ProductImages] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ProductImages_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Color', N'CreatedAt', N'Icon', N'IsActive', N'IsDeleted', N'Name', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] ON;
    EXEC(N'INSERT INTO [Categories] ([Id], [Color], [CreatedAt], [Icon], [IsActive], [IsDeleted], [Name], [UpdatedAt])
    VALUES (1, N''#FFFFFF'', ''0001-01-01T00:00:00.0000000'', N''icon-avg'', CAST(0 AS bit), CAST(0 AS bit), N''Elektronik'', ''0001-01-01T00:00:00.0000000''),
    (2, N''#FFFFFF'', ''0001-01-01T00:00:00.0000000'', N''icon-avg'', CAST(0 AS bit), CAST(0 AS bit), N''Kitap'', ''0001-01-01T00:00:00.0000000''),
    (3, N''#FFFFFF'', ''0001-01-01T00:00:00.0000000'', N''icon-avg'', CAST(0 AS bit), CAST(0 AS bit), N''Giyim'', ''0001-01-01T00:00:00.0000000''),
    (4, N''#FFFFFF'', ''0001-01-01T00:00:00.0000000'', N''icon-avg'', CAST(0 AS bit), CAST(0 AS bit), N''Ev & Yaşam'', ''0001-01-01T00:00:00.0000000''),
    (5, N''#FFFFFF'', ''0001-01-01T00:00:00.0000000'', N''icon-avg'', CAST(0 AS bit), CAST(0 AS bit), N''Spor & Outdoor'', ''0001-01-01T00:00:00.0000000''),
    (6, N''#FFFFFF'', ''0001-01-01T00:00:00.0000000'', N''icon-avg'', CAST(0 AS bit), CAST(0 AS bit), N''Güzellik & Kişisel Bakım'', ''0001-01-01T00:00:00.0000000''),
    (7, N''#FFFFFF'', ''0001-01-01T00:00:00.0000000'', N''icon-avg'', CAST(0 AS bit), CAST(0 AS bit), N''Oyun & Eğlence'', ''0001-01-01T00:00:00.0000000''),
    (8, N''#FFFFFF'', ''0001-01-01T00:00:00.0000000'', N''icon-avg'', CAST(0 AS bit), CAST(0 AS bit), N''Müzik & Film'', ''0001-01-01T00:00:00.0000000''),
    (9, N''#FFFFFF'', ''0001-01-01T00:00:00.0000000'', N''icon-avg'', CAST(0 AS bit), CAST(0 AS bit), N''Ofis & Kırtasiye'', ''0001-01-01T00:00:00.0000000''),
    (10, N''#FFFFFF'', ''0001-01-01T00:00:00.0000000'', N''icon-avg'', CAST(0 AS bit), CAST(0 AS bit), N''Evcil Hayvan'', ''0001-01-01T00:00:00.0000000'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Color', N'CreatedAt', N'Icon', N'IsActive', N'IsDeleted', N'Name', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Categories]'))
        SET IDENTITY_INSERT [Categories] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'IsActive', N'IsDeleted', N'Name', N'NormalizedName', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Roles]'))
        SET IDENTITY_INSERT [Roles] ON;
    EXEC(N'INSERT INTO [Roles] ([Id], [CreatedAt], [Description], [IsActive], [IsDeleted], [Name], [NormalizedName], [UpdatedAt])
    VALUES (1, ''2025-01-01T00:00:00.0000000'', NULL, CAST(1 AS bit), CAST(0 AS bit), N''Admin'', N''ADMIN'', ''2025-01-01T00:00:00.0000000''),
    (2, ''2025-01-01T00:00:00.0000000'', NULL, CAST(1 AS bit), CAST(0 AS bit), N''Seller'', N''SELLER'', ''2025-01-01T00:00:00.0000000''),
    (3, ''2025-01-01T00:00:00.0000000'', NULL, CAST(1 AS bit), CAST(0 AS bit), N''Buyer'', N''BUYER'', ''2025-01-01T00:00:00.0000000'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Description', N'IsActive', N'IsDeleted', N'Name', N'NormalizedName', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Roles]'))
        SET IDENTITY_INSERT [Roles] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Address', N'AddressOther', N'ConcurrencyStamp', N'CreatedAt', N'Email', N'EmailConfirmed', N'FirstName', N'FullName', N'IsActive', N'IsDeleted', N'LastName', N'NormalizedEmail', N'NormalizedUserName', N'Password', N'PasswordHash', N'RefreshToken', N'RefreshTokenExpiryTime', N'ResetPasswordToken', N'RoleId', N'SecurityStamp', N'UpdatedAt', N'UserName') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] ON;
    EXEC(N'INSERT INTO [Users] ([Id], [Address], [AddressOther], [ConcurrencyStamp], [CreatedAt], [Email], [EmailConfirmed], [FirstName], [FullName], [IsActive], [IsDeleted], [LastName], [NormalizedEmail], [NormalizedUserName], [Password], [PasswordHash], [RefreshToken], [RefreshTokenExpiryTime], [ResetPasswordToken], [RoleId], [SecurityStamp], [UpdatedAt], [UserName])
    VALUES (''a1111111-1111-1111-1111-111111111111'', NULL, NULL, N''b213da9a-7ef7-41ee-9727-ae952f025232'', ''2025-01-01T00:00:00.0000000'', N''admin@test.com'', CAST(1 AS bit), NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), NULL, N''ADMIN@TEST.COM'', N''TESTADMIN'', NULL, N''AQAAAAIAAYagAAAAEK75vPpAQnDHXyqmAqyC8iHU4JLmdyI7w4T2DFZMeWU9Y6khOqDwdGh1XB/vCzmJyA=='', NULL, NULL, NULL, 1, N''39422823-ef4d-4ca6-a5b3-6d87d2122da7'', ''2025-01-01T00:00:00.0000000'', N''testadmin''),
    (''a2222222-2222-2222-2222-222222222222'', NULL, NULL, N''4b0a33df-e1e8-4a36-bccc-0d84ad67b3e1'', ''2025-01-01T00:00:00.0000000'', N''seller@test.com'', CAST(1 AS bit), NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), NULL, N''SELLER@TEST.COM'', N''TESTSELLER'', NULL, N''AQAAAAIAAYagAAAAEPaFL8uXg8AAKKd0M22Dw6gNDZlNYw7CBdz6jPSl0sahXz23Cz7tA7681yH4EFAC9w=='', NULL, NULL, NULL, 2, N''6030ee6b-5621-4443-861f-cc2fa7c02fea'', ''2025-01-01T00:00:00.0000000'', N''testseller''),
    (''a3333333-3333-3333-3333-333333333333'', NULL, NULL, N''7a9ac97c-ea65-43fb-a985-6c440f53027f'', ''2025-01-01T00:00:00.0000000'', N''buyer@test.com'', CAST(1 AS bit), NULL, NULL, CAST(1 AS bit), CAST(0 AS bit), NULL, N''BUYER@TEST.COM'', N''TESTBUYER'', NULL, N''AQAAAAIAAYagAAAAEPSwGnR1jVg7RjLXbt6kuY3s6jt/wt/VwbD0v6Vc8UsaQabN2WsImniB5L5foYa+cw=='', NULL, NULL, NULL, 3, N''a369c605-4578-4719-8b69-8f6efa9ab444'', ''2025-01-01T00:00:00.0000000'', N''testbuyer'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Address', N'AddressOther', N'ConcurrencyStamp', N'CreatedAt', N'Email', N'EmailConfirmed', N'FirstName', N'FullName', N'IsActive', N'IsDeleted', N'LastName', N'NormalizedEmail', N'NormalizedUserName', N'Password', N'PasswordHash', N'RefreshToken', N'RefreshTokenExpiryTime', N'ResetPasswordToken', N'RoleId', N'SecurityStamp', N'UpdatedAt', N'UserName') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CategoryId', N'CreatedAt', N'Description', N'DiscountId', N'IsActive', N'IsDeleted', N'Name', N'Price', N'SellerId', N'StockAmount', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Products]'))
        SET IDENTITY_INSERT [Products] ON;
    EXEC(N'INSERT INTO [Products] ([Id], [CategoryId], [CreatedAt], [Description], [DiscountId], [IsActive], [IsDeleted], [Name], [Price], [SellerId], [StockAmount], [UpdatedAt])
    VALUES (1, 1, ''2025-01-01T00:00:00.0000000'', N''Yeni nesil telefon'', NULL, CAST(1 AS bit), CAST(0 AS bit), N''Akıllı Telefon'', 10000.0, ''a2222222-2222-2222-2222-222222222222'', CAST(100 AS tinyint), ''2025-01-01T00:00:00.0000000''),
    (2, 2, ''2025-01-01T00:00:00.0000000'', N''Popüler roman'', NULL, CAST(1 AS bit), CAST(0 AS bit), N''Roman Kitabı'', 150.0, ''a2222222-2222-2222-2222-222222222222'', CAST(200 AS tinyint), ''2025-01-01T00:00:00.0000000''),
    (3, 3, ''2025-01-01T00:00:00.0000000'', N''Pamuklu tişört'', NULL, CAST(1 AS bit), CAST(0 AS bit), N''Tişört'', 50.0, ''a2222222-2222-2222-2222-222222222222'', CAST(245 AS tinyint), ''2025-01-01T00:00:00.0000000'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CategoryId', N'CreatedAt', N'Description', N'DiscountId', N'IsActive', N'IsDeleted', N'Name', N'Price', N'SellerId', N'StockAmount', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Products]'))
        SET IDENTITY_INSERT [Products] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Comment', N'CreatedAt', N'IsActive', N'IsConfirmed', N'IsDeleted', N'ProductId', N'StarCount', N'UpdatedAt', N'UserId') AND [object_id] = OBJECT_ID(N'[ProductComments]'))
        SET IDENTITY_INSERT [ProductComments] ON;
    EXEC(N'INSERT INTO [ProductComments] ([Id], [Comment], [CreatedAt], [IsActive], [IsConfirmed], [IsDeleted], [ProductId], [StarCount], [UpdatedAt], [UserId])
    VALUES (1, N''Bu kitabı çok beğendim!'', ''2025-01-01T00:00:00.0000000'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), 2, CAST(5 AS tinyint), ''2025-01-01T00:00:00.0000000'', ''a1111111-1111-1111-1111-111111111111''),
    (2, N''Oldukça rahattı'', ''2025-01-01T00:00:00.0000000'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit), 3, CAST(4 AS tinyint), ''2025-01-01T00:00:00.0000000'', ''a1111111-1111-1111-1111-111111111111'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Comment', N'CreatedAt', N'IsActive', N'IsConfirmed', N'IsDeleted', N'ProductId', N'StarCount', N'UpdatedAt', N'UserId') AND [object_id] = OBJECT_ID(N'[ProductComments]'))
        SET IDENTITY_INSERT [ProductComments] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'IsActive', N'IsDeleted', N'ProductId', N'UpdatedAt', N'Url') AND [object_id] = OBJECT_ID(N'[ProductImages]'))
        SET IDENTITY_INSERT [ProductImages] ON;
    EXEC(N'INSERT INTO [ProductImages] ([Id], [CreatedAt], [IsActive], [IsDeleted], [ProductId], [UpdatedAt], [Url])
    VALUES (1, ''0001-01-01T00:00:00.0000000'', CAST(0 AS bit), CAST(0 AS bit), 1, ''0001-01-01T00:00:00.0000000'', N''https://picsum.photos/200/300''),
    (2, ''0001-01-01T00:00:00.0000000'', CAST(0 AS bit), CAST(0 AS bit), 2, ''0001-01-01T00:00:00.0000000'', N''https://picsum.photos/200/300''),
    (3, ''0001-01-01T00:00:00.0000000'', CAST(0 AS bit), CAST(0 AS bit), 3, ''0001-01-01T00:00:00.0000000'', N''https://picsum.photos/200/300'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'IsActive', N'IsDeleted', N'ProductId', N'UpdatedAt', N'Url') AND [object_id] = OBJECT_ID(N'[ProductImages]'))
        SET IDENTITY_INSERT [ProductImages] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Orders_UserEntityId] ON [Orders] ([UserEntityId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Orders_UserId] ON [Orders] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProductComments_ProductId] ON [ProductComments] ([ProductId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProductComments_UserId] ON [ProductComments] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProductImages_ProductId] ON [ProductImages] ([ProductId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Products_DiscountId] ON [Products] ([DiscountId]) WHERE [DiscountId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Products_SellerId] ON [Products] ([SellerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Users_RoleId] ON [Users] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250830211321_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250830211321_InitialCreate', N'8.0.18');
END;
GO

COMMIT;
GO

