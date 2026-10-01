using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class UniqueGameName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Games_Name",
                table: "Games",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_Name",
                table: "Games");
        }
    }
}
