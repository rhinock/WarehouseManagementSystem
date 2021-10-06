//using Microsoft.EntityFrameworkCore.Migrations;

//namespace WMS.Migrations
//{
//    public partial class AddConstraints : Migration
//    {
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropIndex(
//                name: "IX_WarehouseItems_WarehouseId",
//                table: "WarehouseItems");

//            migrationBuilder.AlterColumn<string>(
//                name: "Name",
//                table: "Warehouses",
//                nullable: true,
//                oldClrType: typeof(string),
//                oldType: "nvarchar(max)",
//                oldNullable: true);

//            migrationBuilder.AlterColumn<string>(
//                name: "Name",
//                table: "Items",
//                nullable: true,
//                oldClrType: typeof(string),
//                oldType: "nvarchar(max)",
//                oldNullable: true);

//            migrationBuilder.CreateIndex(
//                name: "IX_Warehouses_Name",
//                table: "Warehouses",
//                column: "Name",
//                unique: true,
//                filter: "[Name] IS NOT NULL");

//            migrationBuilder.CreateIndex(
//                name: "IX_WarehouseItems_WarehouseId_ItemId",
//                table: "WarehouseItems",
//                columns: new[] { "WarehouseId", "ItemId" },
//                unique: true);

//            migrationBuilder.CreateIndex(
//                name: "IX_Items_Name",
//                table: "Items",
//                column: "Name",
//                unique: true,
//                filter: "[Name] IS NOT NULL");
//        }

//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropIndex(
//                name: "IX_Warehouses_Name",
//                table: "Warehouses");

//            migrationBuilder.DropIndex(
//                name: "IX_WarehouseItems_WarehouseId_ItemId",
//                table: "WarehouseItems");

//            migrationBuilder.DropIndex(
//                name: "IX_Items_Name",
//                table: "Items");

//            migrationBuilder.AlterColumn<string>(
//                name: "Name",
//                table: "Warehouses",
//                type: "nvarchar(max)",
//                nullable: true,
//                oldClrType: typeof(string),
//                oldNullable: true);

//            migrationBuilder.AlterColumn<string>(
//                name: "Name",
//                table: "Items",
//                type: "nvarchar(max)",
//                nullable: true,
//                oldClrType: typeof(string),
//                oldNullable: true);

//            migrationBuilder.CreateIndex(
//                name: "IX_WarehouseItems_WarehouseId",
//                table: "WarehouseItems",
//                column: "WarehouseId");
//        }
//    }
//}
