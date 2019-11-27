using Microsoft.EntityFrameworkCore.Migrations;

namespace Msz.Migrations
{
    public partial class CreatorOfReceiverAndIsDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Receivers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Receivers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Receivers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Receivers");
        }
    }
}
