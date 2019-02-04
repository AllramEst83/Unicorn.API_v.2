using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Databsase.API.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HornTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HornTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unicorns",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    HornTypeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unicorns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Unicorns_HornTypes_HornTypeId",
                        column: x => x.HornTypeId,
                        principalTable: "HornTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Unicorns_HornTypeId",
                table: "Unicorns",
                column: "HornTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Unicorns");

            migrationBuilder.DropTable(
                name: "HornTypes");
        }
    }
}
