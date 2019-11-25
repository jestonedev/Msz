using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Msz.Migrations
{
    public partial class AddReceiversAndOther : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActual",
                table: "Mszs");

            migrationBuilder.DropColumn(
                name: "PreviousGuid",
                table: "Mszs");

            migrationBuilder.AddColumn<int>(
                name: "NextRevisionId",
                table: "Mszs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousRevisionId",
                table: "Mszs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AssigmentForms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssigmentForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Receivers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Surname = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Patronymic = table.Column<string>(nullable: true),
                    GenderId = table.Column<int>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Snils = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    MszId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    DecisionDate = table.Column<DateTime>(nullable: false),
                    DecisionNumber = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    AssigmentFormId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    EquivalentAmount = table.Column<decimal>(nullable: true),
                    Uuid = table.Column<string>(nullable: false),
                    PrevRevisionId = table.Column<int>(nullable: true),
                    NextRevisionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receivers_AssigmentForms_AssigmentFormId",
                        column: x => x.AssigmentFormId,
                        principalTable: "AssigmentForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Receivers_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Receivers_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Receivers_Mszs_MszId",
                        column: x => x.MszId,
                        principalTable: "Mszs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Receivers_AssigmentFormId",
                table: "Receivers",
                column: "AssigmentFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Receivers_CategoryId",
                table: "Receivers",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Receivers_GenderId",
                table: "Receivers",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Receivers_MszId",
                table: "Receivers",
                column: "MszId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Receivers");

            migrationBuilder.DropTable(
                name: "AssigmentForms");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropColumn(
                name: "NextRevisionId",
                table: "Mszs");

            migrationBuilder.DropColumn(
                name: "PreviousRevisionId",
                table: "Mszs");

            migrationBuilder.AddColumn<bool>(
                name: "IsActual",
                table: "Mszs",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousGuid",
                table: "Mszs",
                nullable: true);
        }
    }
}
