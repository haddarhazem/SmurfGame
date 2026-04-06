using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmurfGame.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSmurfBestTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BestTime",
                table: "Smurfs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BestTime",
                table: "Smurfs");
        }
    }
}
