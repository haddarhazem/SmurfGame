using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmurfGame.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSpeedBuffClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpeedBuffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SpeedBoostAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeedBuffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpeedBuffs_Items_Id",
                        column: x => x.Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpeedBuffs");
        }
    }
}
