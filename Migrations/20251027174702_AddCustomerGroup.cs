using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace yMoi.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Code = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    BankCode = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    AccountNumber = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    AccountHolder = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGroups_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Action = table.Column<string>(type: "VARCHAR", maxLength: 100, nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerActions_CustomerGroups_CustomerGroupId",
                        column: x => x.CustomerGroupId,
                        principalTable: "CustomerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerActions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerActionHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerActionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Field = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    FieldType = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    FromValue = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    ToValue = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerActionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerActionHistories_CustomerActions_CustomerActionId",
                        column: x => x.CustomerActionId,
                        principalTable: "CustomerActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerActionHistories_CustomerActionId",
                table: "CustomerActionHistories",
                column: "CustomerActionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerActions_CustomerGroupId",
                table: "CustomerActions",
                column: "CustomerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerActions_UserId",
                table: "CustomerActions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGroups_CreatedById",
                table: "CustomerGroups",
                column: "CreatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerActionHistories");

            migrationBuilder.DropTable(
                name: "CustomerActions");

            migrationBuilder.DropTable(
                name: "CustomerGroups");
        }
    }
}
