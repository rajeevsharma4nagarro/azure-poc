using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCD.Services.CartAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShoppingCartDetailsTableWithPriceColumnsInActualTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "cartDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "cartDetails");
        }
    }
}
