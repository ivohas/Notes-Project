using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Data.Migrations
{
    public partial class trashEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TrashId",
                table: "Notes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Trash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trash", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trash_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_TrashId",
                table: "Notes",
                column: "TrashId");

            migrationBuilder.CreateIndex(
                name: "IX_Trash_UserId",
                table: "Trash",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Trash_TrashId",
                table: "Notes",
                column: "TrashId",
                principalTable: "Trash",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Trash_TrashId",
                table: "Notes");

            migrationBuilder.DropTable(
                name: "Trash");

            migrationBuilder.DropIndex(
                name: "IX_Notes_TrashId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "TrashId",
                table: "Notes");
        }
    }
}
