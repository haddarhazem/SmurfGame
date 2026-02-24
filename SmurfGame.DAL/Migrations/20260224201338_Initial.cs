using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmurfGame.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Creatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Health = table.Column<int>(type: "int", nullable: false),
                    MaxHealth = table.Column<int>(type: "int", nullable: false),
                    CreatureType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    AttackDamage = table.Column<int>(type: "int", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    IsInForest = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthBoost = table.Column<int>(type: "int", nullable: false),
                    IsConsumed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ItemType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    BoostMultiplier = table.Column<int>(type: "int", nullable: true, defaultValue: 2),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Creatures_Position",
                table: "Creatures",
                columns: new[] { "X", "Y" });

            migrationBuilder.CreateIndex(
                name: "IX_Items_Position",
                table: "Items",
                columns: new[] { "X", "Y" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Creatures");

            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
