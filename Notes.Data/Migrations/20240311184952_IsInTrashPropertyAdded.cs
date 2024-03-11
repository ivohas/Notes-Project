using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Data.Migrations
{
    public partial class IsInTrashPropertyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInTrash",
                table: "Notes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInTrash",
                table: "Notes");
        }
    }
}
