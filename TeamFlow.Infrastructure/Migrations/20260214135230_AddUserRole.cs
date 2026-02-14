using Microsoft.EntityFrameworkCore.Migrations;
using TeamFlow.Domain.Entities;
using TeamFlow.Domain.Enums;

#nullable disable

namespace TeamFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role_New",
                table: "Users",
                nullable: false,
                defaultValue: (int)UserRole.User);

           migrationBuilder.Sql(@"
            UPDATE ""Users"" SET ""Role_New"" = CASE ""Role""
                WHEN 'User'   THEN 0
                WHEN 'Admin' THEN 1
                ELSE 0
                END
            ");

            migrationBuilder.DropColumn("Role", "Users");

            migrationBuilder.RenameColumn("Role_New", "Users", "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>("Role_New", "Users", nullable: false, defaultValue: "User");

            migrationBuilder.Sql(@"
                UPDATE ""Users"" SET ""Role_New"" = CASE ""Role""
                    WHEN 0 THEN 'User'
                    WHEN 1 THEN 'Admin'
                END
            ");

            migrationBuilder.DropColumn("Role", "Users");
            migrationBuilder.RenameColumn("Role_New", "Users", "Role");
        }
    }
}
