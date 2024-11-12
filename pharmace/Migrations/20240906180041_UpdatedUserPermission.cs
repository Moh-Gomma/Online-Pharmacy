using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pharmace.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "userpermations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordToken",
                table: "userpermations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordTokenExpiration",
                table: "userpermations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "userpermations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "userpermations");

            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "userpermations");

            migrationBuilder.DropColumn(
                name: "ResetPasswordTokenExpiration",
                table: "userpermations");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "userpermations");
        }
    }
}
