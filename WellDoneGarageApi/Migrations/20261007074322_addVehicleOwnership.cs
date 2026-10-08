using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellDoneGarageApi.Migrations
{
    /// <inheritdoc />
    public partial class addVehicleOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleOwnership",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleOwnership", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleOwnership_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleOwnership_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleOwnership_CustomerId",
                table: "VehicleOwnership",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleOwnership_VehicleId",
                table: "VehicleOwnership",
                column: "VehicleId",
                unique: true,
                filter: "[EndDate] IS NULL");
            // Backfill ownership history for existing vehicles
            migrationBuilder.Sql(@"
        INSERT INTO VehicleOwnership
        (
            VehicleId,
            CustomerId,
            StartDate,
            EndDate,
            CreatedAt,
            CreatedBy,
            UpdatedAt,
            UpdatedBy,
            IsDeleted
        )
        SELECT
            Id,
            CustomerId,
            CreatedAt,
            NULL,
            CreatedAt,
            CreatedBy,
            UpdatedAt,
            UpdatedBy,
            IsDeleted
        FROM Vehicles;
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleOwnership");
        }
    }
}
