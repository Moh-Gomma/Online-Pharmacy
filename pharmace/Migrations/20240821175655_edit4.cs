using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pharmace.Migrations
{
    /// <inheritdoc />
    public partial class edit4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "count",
                table: "Proudects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "count",
                table: "Proudects");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Categories");
        }
    }
}
