using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    public partial class AddVillaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Villas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Occupancy = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amenity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaInSqFt = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villas", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "AreaInSqFt", "CreatedDate", "Details", "ImageURL", "Name", "Occupancy", "Rate", "UpdatedDate" },
                values: new object[] { 1, "", 550, new DateTime(2022, 10, 9, 22, 31, 38, 496, DateTimeKind.Local).AddTicks(1898), "This is a royal villa.", "", "Royal Villa", 100, 200.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "AreaInSqFt", "CreatedDate", "Details", "ImageURL", "Name", "Occupancy", "Rate", "UpdatedDate" },
                values: new object[] { 2, "", 50, new DateTime(2022, 10, 9, 22, 31, 38, 496, DateTimeKind.Local).AddTicks(1917), "This is a soda villa.", "", "Coca Cola Villa", 150, 100.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "AreaInSqFt", "CreatedDate", "Details", "ImageURL", "Name", "Occupancy", "Rate", "UpdatedDate" },
                values: new object[] { 3, "", 200, new DateTime(2022, 10, 9, 22, 31, 38, 496, DateTimeKind.Local).AddTicks(1919), "This is the ice cream villa.", "", "Ice Cream Villa", 150, 150.0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Villas");
        }
    }
}
