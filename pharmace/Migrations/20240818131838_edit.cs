using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pharmace.Migrations
{
    /// <inheritdoc />
    public partial class edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fname",
                table: "userpermations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "lname",
                table: "userpermations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fname",
                table: "userpermations");

            migrationBuilder.DropColumn(
                name: "lname",
                table: "userpermations");
        }
    }
}
