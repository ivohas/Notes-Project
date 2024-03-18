using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Data.Migrations
{
    public partial class ChangeNameIfProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserNote_Notes_FavoriteWatchesId",
                table: "ApplicationUserNote");

            migrationBuilder.RenameColumn(
                name: "FavoriteWatchesId",
                table: "ApplicationUserNote",
                newName: "FavoriteNotesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserNote_Notes_FavoriteNotesId",
                table: "ApplicationUserNote",
                column: "FavoriteNotesId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserNote_Notes_FavoriteNotesId",
                table: "ApplicationUserNote");

            migrationBuilder.RenameColumn(
                name: "FavoriteNotesId",
                table: "ApplicationUserNote",
                newName: "FavoriteWatchesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserNote_Notes_FavoriteWatchesId",
                table: "ApplicationUserNote",
                column: "FavoriteWatchesId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
