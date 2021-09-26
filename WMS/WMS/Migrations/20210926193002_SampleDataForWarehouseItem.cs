using Microsoft.EntityFrameworkCore.Migrations;

namespace WMS.Migrations
{
    public partial class SampleDataForWarehouseItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WarehouseItems",
                columns: new[] { "Id", "Count", "ItemId", "WarehouseId" },
                values: new object[] { 1L, 50L, 1L, 1L });

            migrationBuilder.InsertData(
                table: "WarehouseItems",
                columns: new[] { "Id", "Count", "ItemId", "WarehouseId" },
                values: new object[] { 2L, 5000L, 2L, 2L });

            migrationBuilder.InsertData(
                table: "WarehouseItems",
                columns: new[] { "Id", "Count", "ItemId", "WarehouseId" },
                values: new object[] { 3L, 500000L, 3L, 3L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WarehouseItems",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "WarehouseItems",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "WarehouseItems",
                keyColumn: "Id",
                keyValue: 3L);
        }
    }
}
