using Microsoft.EntityFrameworkCore.Migrations;

namespace Msz.Migrations
{
    public partial class UpdateAclUserPrivilegesPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AclUserPrivilege",
                table: "AclUserPrivilege");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AclUserPrivilege",
                table: "AclUserPrivilege",
                columns: new[] { "UserId", "PrivilegeId", "MszId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AclUserPrivilege",
                table: "AclUserPrivilege");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AclUserPrivilege",
                table: "AclUserPrivilege",
                columns: new[] { "UserId", "PrivilegeId" });
        }
    }
}
