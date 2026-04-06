using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmurfGame.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAzraelClass2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Araels",
                table: "Araels");

            migrationBuilder.RenameTable(
                name: "Araels",
                newName: "Azraels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Azraels",
                table: "Azraels",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Azraels",
                table: "Azraels");

            migrationBuilder.RenameTable(
                name: "Azraels",
                newName: "Araels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Araels",
                table: "Araels",
                column: "Id");
        }
    }
}
