using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SCD.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class ProductDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "Category", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Clothing", "Nice t-shirt", "/images/110040ba-d84e-48bc-b12a-3953a8c147b4.png", "T-shirt", 499.0 },
                    { 2, "Clothing", "Chipest jeans", "/images/404d8e91-5765-4ae6-ae8d-5f9f9c4d087e.png", "Jeans", 1199.0 },
                    { 3, "Clothing", "Stylist jacket", "/images/872eb7f6-3f47-46b1-baa1-96e3bf567b78.png", "Jacket", 1499.0 },
                    { 4, "Footwear", "Cool shoes", "/images/e1e83066-802d-40b8-bdbc-dacbc63a848b.png", "Shoes", 1999.0 },
                    { 5, "Footwear", "Princes style", "/images/98bb4ebf-3794-4a3e-a2d0-4ef4e0380f9a.png", "Sandals", 799.0 },
                    { 6, "Accessories", "Looks rocking ", "/images/cbee0ecf-19dc-400a-892f-35122d43b00f.png", "Cap", 299.0 },
                    { 7, "Accessories", "Awesome watch", "/images/bed1a22a-4147-4546-8aab-06301a3df2cd.png", "Watch", 1599.0 },
                    { 8, "Accessories", "Compact bag", "/images/3202cac7-8d3c-4d93-8235-a6fd7f1a85ed.png", "Laptop Bag", 899.0 },
                    { 9, "Electronics", "High base", "/images/b4c2f781-04f3-4add-aabf-89943f03ee07.png", "Headphones", 2499.0 },
                    { 10, "Electronics", "Huge features", "/images/b3cddf57-7051-43b5-b8f7-915f9913b30e.png", "Laptop", 15999.0 },
                    { 11, "Footwear", "Cool lofer", "/images/b3cddf57-7051-43b5-b8f7-915f9913b30e.png", "Lofer", 15999.0 },
                    { 12, "Electronics", "Smart induction", "/images/442ad0e3-ddd2-474d-8877-660bc7114f0f.png", "Induction", 15999.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
