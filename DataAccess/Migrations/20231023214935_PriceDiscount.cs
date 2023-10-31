using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PriceDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price50",
                table: "Products",
                newName: "DiscountPercent");

            migrationBuilder.RenameColumn(
                name: "Price100",
                table: "Products",
                newName: "DiscountAmount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscountPercent",
                table: "Products",
                newName: "Price50");

            migrationBuilder.RenameColumn(
                name: "DiscountAmount",
                table: "Products",
                newName: "Price100");
        }
    }
}
