using Microsoft.EntityFrameworkCore.Migrations;

namespace WMS.Migrations
{
    /// <summary>
    /// Миграция №1
    /// </summary>
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    MaximumItems = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemWarehouses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseId = table.Column<long>(nullable: false),
                    ItemId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemWarehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemWarehouses_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemWarehouses_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "MaximumItems", "Name" },
                values: new object[] { 1L, 100L, "Склад миниатюрный" });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "MaximumItems", "Name" },
                values: new object[] { 2L, 10000L, "Склад приличный" });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "MaximumItems", "Name" },
                values: new object[] { 3L, 1000000L, "Склад здоровенный" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemWarehouses_ItemId",
                table: "ItemWarehouses",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemWarehouses_WarehouseId",
                table: "ItemWarehouses",
                column: "WarehouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemWarehouses");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Warehouses");
        }
    }
}
