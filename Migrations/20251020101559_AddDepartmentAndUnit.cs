using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace yMoi.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentAndUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    Code = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    Note = table.Column<string>(type: "VARCHAR", maxLength: 1000, nullable: true),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    Code = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    Note = table.Column<string>(type: "VARCHAR", maxLength: 1000, nullable: true),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Units_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Code = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    UnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    ImportPrice = table.Column<long>(type: "INTEGER", nullable: true),
                    ImportPriceUnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    OfficialPrice = table.Column<long>(type: "INTEGER", nullable: true),
                    OfficialPriceUnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantity = table.Column<long>(type: "INTEGER", nullable: true),
                    Supplier = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    PackingSpecification = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Manufacturer = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "VARCHAR", maxLength: 1000, nullable: true),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicines_Units_ImportPriceUnitId",
                        column: x => x.ImportPriceUnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Medicines_Units_OfficialPriceUnitId",
                        column: x => x.OfficialPriceUnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Medicines_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Medicines_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Code = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    UnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    Type = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    DepartmentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ImportPrice = table.Column<long>(type: "INTEGER", nullable: true),
                    ImportPriceUnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    OfficialPrice = table.Column<long>(type: "INTEGER", nullable: true),
                    OfficialPriceUnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReferenceLimit1 = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    ReferenceLimit2 = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    TurnaroundTime = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    ToolFeatures = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "VARCHAR", maxLength: 1000, nullable: true),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Services_Units_ImportPriceUnitId",
                        column: x => x.ImportPriceUnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Services_Units_OfficialPriceUnitId",
                        column: x => x.OfficialPriceUnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Services_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Services_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicineFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MedicineId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR", maxLength: 500, nullable: false),
                    Url = table.Column<string>(type: "VARCHAR", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineFiles_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR", maxLength: 500, nullable: false),
                    Url = table.Column<string>(type: "VARCHAR", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceFiles_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CreatedById",
                table: "Departments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineFiles_MedicineId",
                table: "MedicineFiles",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_CreatedById",
                table: "Medicines",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_ImportPriceUnitId",
                table: "Medicines",
                column: "ImportPriceUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_OfficialPriceUnitId",
                table: "Medicines",
                column: "OfficialPriceUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_UnitId",
                table: "Medicines",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFiles_ServiceId",
                table: "ServiceFiles",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_CreatedById",
                table: "Services",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Services_DepartmentId",
                table: "Services",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ImportPriceUnitId",
                table: "Services",
                column: "ImportPriceUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_OfficialPriceUnitId",
                table: "Services",
                column: "OfficialPriceUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_UnitId",
                table: "Services",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_CreatedById",
                table: "Units",
                column: "CreatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicineFiles");

            migrationBuilder.DropTable(
                name: "ServiceFiles");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Units");
        }
    }
}
