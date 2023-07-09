using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPIUAI.Migrations
{
    /// <inheritdoc />
    public partial class AdminData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] ON;
INSERT INTO [Roles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
VALUES (N'992e1348-65d0-4d08-8bf9-398b417c38ff', NULL, N'Admin', N'ADMIN');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Email', N'EmailConfirmed', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
    SET IDENTITY_INSERT [Usuarios] ON;
INSERT INTO [Usuarios] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName])
VALUES (N'834f154b-b004-4707-b7d6-6e7b88d1a537', 0, N'75fc9bca-3185-48cc-801c-9f8492e0b65a', N'gabriel@mail.com', CAST(0 AS bit), CAST(0 AS bit), NULL, N'GABRIEL@MAIL.COM', N'GABRIEL@MAIL.COM', N'AQAAAAIAAYagAAAAEFBJ7NWTlbNDLd+9Au2cjnMvkprN3lhugkdH6SNL3OK4yP8ILjnAbQgAghoZ8hyCyg==', NULL, CAST(0 AS bit), N'5fe83140-9e38-4786-83d6-92c5bae92300', CAST(0 AS bit), N'gabriel@mail.com');       
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Email', N'EmailConfirmed', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
    SET IDENTITY_INSERT [Usuarios] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'UserId') AND [object_id] = OBJECT_ID(N'[UsuarioClaims]'))
    SET IDENTITY_INSERT [UsuarioClaims] ON;
INSERT INTO [UsuarioClaims] ([Id], [ClaimType], [ClaimValue], [UserId])
VALUES (1, N'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', N'Admin', N'834f154b-b004-4707-b7d6-6e7b88d1a537');   
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'UserId') AND [object_id] = OBJECT_ID(N'[UsuarioClaims]'))
    SET IDENTITY_INSERT [UsuarioClaims] OFF;
GO
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                            table: "Roles",
                            keyColumn: "Id",
                            keyValue: "992e1348-65d0-4d08-8bf9-398b417c38ff");

            migrationBuilder.DeleteData(
                table: "UsuarioClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: "834f154b-b004-4707-b7d6-6e7b88d1a537");
        }
    }
}
