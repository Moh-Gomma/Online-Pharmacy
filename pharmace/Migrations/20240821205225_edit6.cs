using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pharmace.Migrations
{
    /// <inheritdoc />
    public partial class edit6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "orderprodects",
                columns: table => new
                {
                    orderId = table.Column<int>(type: "int", nullable: false),
                    proudectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderprodects", x => new { x.proudectId, x.orderId });
                    table.ForeignKey(
                        name: "FK_orderprodects_Orders_orderId",
                        column: x => x.orderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orderprodects_Proudects_proudectId",
                        column: x => x.proudectId,
                        principalTable: "Proudects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orderprodects_orderId",
                table: "orderprodects",
                column: "orderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orderprodects");
        }
    }
}
