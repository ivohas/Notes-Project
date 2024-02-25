using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Data.Migrations
{
    public partial class Notebooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NotebookId",
                table: "Notes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_NotebookId",
                table: "Notes",
                column: "NotebookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Notebooks_NotebookId",
                table: "Notes",
                column: "NotebookId",
                principalTable: "Notebooks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Notebooks_NotebookId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_NotebookId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "NotebookId",
                table: "Notes");
        }
    }
}
