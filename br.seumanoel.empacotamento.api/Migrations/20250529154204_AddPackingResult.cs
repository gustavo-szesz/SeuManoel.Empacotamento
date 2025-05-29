using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace br.seumanoel.empacotamento.api.Migrations
{
    /// <inheritdoc />
    public partial class AddPackingResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackingResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PackedBoxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackingResultId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackedBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackedBoxes_PackingResults_PackingResultId",
                        column: x => x.PackingResultId,
                        principalTable: "PackingResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackedProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackedBoxId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackedProducts_PackedBoxes_PackedBoxId",
                        column: x => x.PackedBoxId,
                        principalTable: "PackedBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackedBoxes_PackingResultId",
                table: "PackedBoxes",
                column: "PackingResultId");

            migrationBuilder.CreateIndex(
                name: "IX_PackedProducts_PackedBoxId",
                table: "PackedProducts",
                column: "PackedBoxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackedProducts");

            migrationBuilder.DropTable(
                name: "PackedBoxes");

            migrationBuilder.DropTable(
                name: "PackingResults");
        }
    }
}
