using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Msz.Migrations
{
    public partial class Acl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AclPrivilege",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AclPrivilege", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AclUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AclUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AclUserPrivilege",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    PrivilegeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AclUserPrivilege", x => new { x.UserId, x.PrivilegeId });
                    table.ForeignKey(
                        name: "FK_AclUserPrivilege_AclPrivilege_PrivilegeId",
                        column: x => x.PrivilegeId,
                        principalTable: "AclPrivilege",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AclUserPrivilege_AclUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AclUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AclPrivilege",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Изменение" });

            migrationBuilder.CreateIndex(
                name: "IX_AclUserPrivilege_PrivilegeId",
                table: "AclUserPrivilege",
                column: "PrivilegeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AclUserPrivilege");

            migrationBuilder.DropTable(
                name: "AclPrivilege");

            migrationBuilder.DropTable(
                name: "AclUser");
        }
    }
}
