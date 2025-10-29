using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace yMoi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFieldsInWardV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "WardV2s");

            migrationBuilder.DropColumn(
                name: "WardCode",
                table: "WardV2s");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "WardV2s",
                type: "VARCHAR",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "WardCode",
                table: "WardV2s",
                type: "INTEGER",
                nullable: true);
        }
    }
}
