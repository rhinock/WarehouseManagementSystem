using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WMS.DataAccess.Migrations
{
    public partial class postgres01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    MaximumItems = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Count = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseItems_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1L, "Карандаш", 10.00m },
                    { 2L, "Ручка", 20.00m },
                    { 3L, "Фломастер", 30.00m }
                });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "Id", "MaximumItems", "Name" },
                values: new object[,]
                {
                    { 1L, 100L, "Склад миниатюрный" },
                    { 2L, 10000L, "Склад приличный" },
                    { 3L, 1000000L, "Склад здоровенный" }
                });

            migrationBuilder.InsertData(
                table: "WarehouseItems",
                columns: new[] { "Id", "Count", "ItemId", "WarehouseId" },
                values: new object[,]
                {
                    { 1L, 50L, 1L, 1L },
                    { 2L, 5000L, 2L, 2L },
                    { 3L, 500000L, 3L, 3L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name",
                table: "Items",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_ItemId",
                table: "WarehouseItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_WarehouseId_ItemId",
                table: "WarehouseItems",
                columns: new[] { "WarehouseId", "ItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Name",
                table: "Warehouses",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarehouseItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Warehouses");
        }
    }
}
