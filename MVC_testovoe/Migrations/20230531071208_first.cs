using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC_testovoe.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Izdels",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izdels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IzdelUpId = table.Column<long>(type: "bigint", nullable: false),
                    IzdelId = table.Column<long>(type: "bigint", nullable: false),
                    kol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Links_Izdels_IzdelId",
                        column: x => x.IzdelId,
                        principalTable: "Izdels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Links_Izdels_IzdelUpId",
                        column: x => x.IzdelUpId,
                        principalTable: "Izdels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Links_IzdelId",
                table: "Links",
                column: "IzdelId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_IzdelUpId",
                table: "Links",
                column: "IzdelUpId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "Izdels");
        }
    }
}
