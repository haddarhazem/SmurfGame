using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmurfGame.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RefactorEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LegCount",
                table: "Spiders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlySpeed",
                table: "BzzFlies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegCount",
                table: "Spiders");

            migrationBuilder.DropColumn(
                name: "FlySpeed",
                table: "BzzFlies");
        }
    }
}
