using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmurfGame.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TPT_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoostMultiplier",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "AttackDamage",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "CreatureType",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "IsInForest",
                table: "Creatures");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Creatures");

            migrationBuilder.CreateTable(
                name: "Berries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Berries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Berries_Items_Id",
                        column: x => x.Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BluePotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BluePotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BluePotions_Items_Id",
                        column: x => x.Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bugs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AttackDamage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bugs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bugs_Creatures_Id",
                        column: x => x.Id,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RedPotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BoostMultiplier = table.Column<int>(type: "int", nullable: false, defaultValue: 2)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedPotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RedPotions_Items_Id",
                        column: x => x.Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Smurfs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsInForest = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Smurfs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Smurfs_Creatures_Id",
                        column: x => x.Id,
                        principalTable: "Creatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BzzFlies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BzzFlies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BzzFlies_Bugs_Id",
                        column: x => x.Id,
                        principalTable: "Bugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Spiders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spiders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spiders_Bugs_Id",
                        column: x => x.Id,
                        principalTable: "Bugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Berries");

            migrationBuilder.DropTable(
                name: "BluePotions");

            migrationBuilder.DropTable(
                name: "BzzFlies");

            migrationBuilder.DropTable(
                name: "RedPotions");

            migrationBuilder.DropTable(
                name: "Smurfs");

            migrationBuilder.DropTable(
                name: "Spiders");

            migrationBuilder.DropTable(
                name: "Bugs");

            migrationBuilder.AddColumn<int>(
                name: "BoostMultiplier",
                table: "Items",
                type: "int",
                nullable: true,
                defaultValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "ItemType",
                table: "Items",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AttackDamage",
                table: "Creatures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatureType",
                table: "Creatures",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsInForest",
                table: "Creatures",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Creatures",
                type: "int",
                nullable: true,
                defaultValue: 1);
        }
    }
}
