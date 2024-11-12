using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pharmace.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Pricecycle",
                table: "Proudects",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pricecycle",
                table: "Proudects");
        }
    }
}
