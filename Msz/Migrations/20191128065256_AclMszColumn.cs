using Microsoft.EntityFrameworkCore.Migrations;

namespace Msz.Migrations
{
    public partial class AclMszColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AclUserPrivilege_AclUser_UserId",
                table: "AclUserPrivilege");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AclUser",
                table: "AclUser");

            migrationBuilder.RenameTable(
                name: "AclUser",
                newName: "AclUsers");

            migrationBuilder.AddColumn<int>(
                name: "MszId",
                table: "AclUserPrivilege",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AclUsers",
                table: "AclUsers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AclUserPrivilege_MszId",
                table: "AclUserPrivilege",
                column: "MszId");

            migrationBuilder.AddForeignKey(
                name: "FK_AclUserPrivilege_Mszs_MszId",
                table: "AclUserPrivilege",
                column: "MszId",
                principalTable: "Mszs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AclUserPrivilege_AclUsers_UserId",
                table: "AclUserPrivilege",
                column: "UserId",
                principalTable: "AclUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AclUserPrivilege_Mszs_MszId",
                table: "AclUserPrivilege");

            migrationBuilder.DropForeignKey(
                name: "FK_AclUserPrivilege_AclUsers_UserId",
                table: "AclUserPrivilege");

            migrationBuilder.DropIndex(
                name: "IX_AclUserPrivilege_MszId",
                table: "AclUserPrivilege");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AclUsers",
                table: "AclUsers");

            migrationBuilder.DropColumn(
                name: "MszId",
                table: "AclUserPrivilege");

            migrationBuilder.RenameTable(
                name: "AclUsers",
                newName: "AclUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AclUser",
                table: "AclUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AclUserPrivilege_AclUser_UserId",
                table: "AclUserPrivilege",
                column: "UserId",
                principalTable: "AclUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
