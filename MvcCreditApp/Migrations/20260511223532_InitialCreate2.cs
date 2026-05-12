using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcCreditApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Varieties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Crop = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Breeder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Varieties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VarietyInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VarietyId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Germination = table.Column<int>(type: "int", nullable: false),
                    Productivity = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VarietyInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VarietyInfos_Varieties_VarietyId",
                        column: x => x.VarietyId,
                        principalTable: "Varieties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VarietyInfos_VarietyId",
                table: "VarietyInfos",
                column: "VarietyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VarietyInfos");

            migrationBuilder.DropTable(
                name: "Varieties");
        }
    }
}
