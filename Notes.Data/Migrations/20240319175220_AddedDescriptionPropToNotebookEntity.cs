using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Data.Migrations
{
    public partial class AddedDescriptionPropToNotebookEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserNote");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Notebooks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Notebooks");

            migrationBuilder.CreateTable(
                name: "ApplicationUserNote",
                columns: table => new
                {
                    FavoriteNotesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersWhoFavoritedId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserNote", x => new { x.FavoriteNotesId, x.UsersWhoFavoritedId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserNote_AspNetUsers_UsersWhoFavoritedId",
                        column: x => x.UsersWhoFavoritedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserNote_Notes_FavoriteNotesId",
                        column: x => x.FavoriteNotesId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserNote_UsersWhoFavoritedId",
                table: "ApplicationUserNote",
                column: "UsersWhoFavoritedId");
        }
    }
}
