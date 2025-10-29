using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace yMoi.Migrations
{
    /// <inheritdoc />
    public partial class ProvinceV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProvinceV2s",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvinceV2s", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WardV2s",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: false),
                    WardCode = table.Column<int>(type: "INTEGER", nullable: true),
                    ProvinceV2Id = table.Column<int>(type: "INTEGER", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WardV2s", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WardV2s_ProvinceV2s_ProvinceV2Id",
                        column: x => x.ProvinceV2Id,
                        principalTable: "ProvinceV2s",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WardV2s_ProvinceV2Id",
                table: "WardV2s",
                column: "ProvinceV2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WardV2s");

            migrationBuilder.DropTable(
                name: "ProvinceV2s");
        }
    }
}
