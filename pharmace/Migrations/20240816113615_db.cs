using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pharmace.Migrations
{
    /// <inheritdoc />
    public partial class db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "userpermations");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "userpermations");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Totalprice = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_userpermations_userId",
                        column: x => x.userId,
                        principalTable: "userpermations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Proudects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proudects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proudects_Categories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    proudectId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => new { x.proudectId, x.userId });
                    table.ForeignKey(
                        name: "FK_Carts_Proudects_proudectId",
                        column: x => x.proudectId,
                        principalTable: "Proudects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_userpermations_userId",
                        column: x => x.userId,
                        principalTable: "userpermations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_userId",
                table: "Carts",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_userId",
                table: "Orders",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Proudects_categoryId",
                table: "Proudects",
                column: "categoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Proudects");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "userpermations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "userpermations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
