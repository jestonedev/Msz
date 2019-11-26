using Microsoft.EntityFrameworkCore.Migrations;

namespace Msz.Migrations
{
    public partial class AddPreviousGuidMsz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Mszs",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreviousGuid",
                table: "Mszs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Mszs");

            migrationBuilder.DropColumn(
                name: "PreviousGuid",
                table: "Mszs");
        }
    }
}
