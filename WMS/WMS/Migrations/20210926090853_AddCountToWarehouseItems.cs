using Microsoft.EntityFrameworkCore.Migrations;

namespace WMS.Migrations
{
    public partial class AddCountToWarehouseItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Count",
                table: "WarehouseItems",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "WarehouseItems");
        }
    }
}
