using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace yMoi.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerActions_CustomerGroups_CustomerGroupId",
                table: "CustomerActions");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerGroupId",
                table: "CustomerActions",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "CustomerActions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Code = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Dob = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    Job = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    ProvinceId = table.Column<int>(type: "INTEGER", nullable: true),
                    DistrictId = table.Column<int>(type: "INTEGER", nullable: true),
                    WardId = table.Column<int>(type: "INTEGER", nullable: true),
                    Address = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    PostalCode = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Country = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Language = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Religion = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Nationality = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    IdentityCardNumber = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    EducationalLevel = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    MaritalStatus = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Gender = table.Column<int>(type: "INTEGER", nullable: true),
                    RelativeName = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Relationship = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    RelationshipAddress = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    RelativeProvinceId = table.Column<int>(type: "INTEGER", nullable: true),
                    RelativeDistrictId = table.Column<int>(type: "INTEGER", nullable: true),
                    RelativeWardId = table.Column<int>(type: "INTEGER", nullable: true),
                    RelativeCountry = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    RelativePostalCode = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    RelativePhone = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    BankCode = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    AccountNumber = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    AccountHolder = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    CustomerGroupId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_CustomerGroups_CustomerGroupId",
                        column: x => x.CustomerGroupId,
                        principalTable: "CustomerGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerActions_CustomerId",
                table: "CustomerActions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatedById",
                table: "Customers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerGroupId",
                table: "Customers",
                column: "CustomerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerActions_CustomerGroups_CustomerGroupId",
                table: "CustomerActions",
                column: "CustomerGroupId",
                principalTable: "CustomerGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerActions_Customers_CustomerId",
                table: "CustomerActions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerActions_CustomerGroups_CustomerGroupId",
                table: "CustomerActions");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerActions_Customers_CustomerId",
                table: "CustomerActions");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerActions_CustomerId",
                table: "CustomerActions");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CustomerActions");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerGroupId",
                table: "CustomerActions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerActions_CustomerGroups_CustomerGroupId",
                table: "CustomerActions",
                column: "CustomerGroupId",
                principalTable: "CustomerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
