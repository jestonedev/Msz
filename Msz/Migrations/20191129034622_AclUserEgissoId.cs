using Microsoft.EntityFrameworkCore.Migrations;

namespace Msz.Migrations
{
    public partial class AclUserEgissoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EgissoId",
                table: "AclUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EgissoId",
                table: "AclUsers");
        }
    }
}
