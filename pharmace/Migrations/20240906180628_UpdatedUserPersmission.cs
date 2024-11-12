using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pharmace.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserPersmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "userpermations");

            migrationBuilder.RenameColumn(
                name: "ResetPasswordTokenExpiration",
                table: "userpermations",
                newName: "VerificationTokenExpiry");

            migrationBuilder.AlterColumn<string>(
                name: "ResetPasswordToken",
                table: "userpermations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordTokenExpiry",
                table: "userpermations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationToken",
                table: "userpermations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordTokenExpiry",
                table: "userpermations");

            migrationBuilder.DropColumn(
                name: "VerificationToken",
                table: "userpermations");

            migrationBuilder.RenameColumn(
                name: "VerificationTokenExpiry",
                table: "userpermations",
                newName: "ResetPasswordTokenExpiration");

            migrationBuilder.AlterColumn<string>(
                name: "ResetPasswordToken",
                table: "userpermations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "userpermations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
