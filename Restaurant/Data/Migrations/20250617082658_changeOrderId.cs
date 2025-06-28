using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeOrderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Orders",
                newName: "OrderId");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Tomato, lettuce with Beef", "Beef Sandwich" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Rich Cheese Sandwich with Tomato Slices", "Cheese Sandwich" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Orders",
                newName: "Id");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Fresh romaine lettuce with Caesar dressing", "Caesar Salad" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Rich chocolate cake with creamy frosting", "Chocolate Cake" });
        }
    }
}
