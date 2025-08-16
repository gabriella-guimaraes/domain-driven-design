using Microsoft.EntityFrameworkCore.Migrations;

namespace ContainRs.Api.Identity;

public static class MigrationBuilderExtensions
{
    public static void SeedIdentityData(this MigrationBuilder migrationBuilder)
    {
        string roleSuporte = "Suporte";
        string[] roleNames = [roleSuporte,"Cliente","Comercial","Admin"];
        string email = "suporte@email.org";

        // Cria roles
        foreach(var roleName in roleNames)
        {
            migrationBuilder.Sql($@"
                IF NOT EXISTS (SELECT 1 FROM [Identity].AspNetRoles WHERE Name = '{roleName}')
                BEGIN
                    INSERT INTO [Identity].AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) 
                    VALUES (NEWID(), '{roleName}', UPPER('{roleName}'), NEWID())
                END");
        }

        // Cria usuário inicial de suporte
        migrationBuilder.Sql($@"
            IF NOT EXISTS (SELECT 1 FROM [Identity].AspNetUsers WHERE Email = '{email}')
            BEGIN
                DECLARE @UserId UNIQUEIDENTIFIER = NEWID();
                INSERT INTO [Identity].AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, PasswordHash, SecurityStamp, ConcurrencyStamp, AccessFailedCount) 
                VALUES (@UserId, '{email}', UPPER('{email}'), '{email}', UPPER('{email}'), 1, 1, 0, 0,
                    'AQAAAAIAAYagAAAAEPa7WUxVMskcxZNjU9sJFWCxQAn3wX4eJuALj5mxvm8wrsMkxx7wqiT/DWmKaGNGDA==', 
                    NEWID(), NEWID(), 0);

                INSERT INTO [Identity].AspNetUserRoles (UserId, RoleId) 
                SELECT @UserId, Id FROM [Identity].AspNetRoles WHERE Name = '{roleSuporte}'
            END");
    }
}
