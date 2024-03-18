using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Data.Migrations
{
    public partial class AddedPropToUserAndNoteForFavouriteWatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserNote",
                columns: table => new
                {
                    FavoriteWatchesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersWhoFavoritedId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserNote", x => new { x.FavoriteWatchesId, x.UsersWhoFavoritedId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserNote_AspNetUsers_UsersWhoFavoritedId",
                        column: x => x.UsersWhoFavoritedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserNote_Notes_FavoriteWatchesId",
                        column: x => x.FavoriteWatchesId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserNote_UsersWhoFavoritedId",
                table: "ApplicationUserNote",
                column: "UsersWhoFavoritedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserNote");
        }
    }
}
