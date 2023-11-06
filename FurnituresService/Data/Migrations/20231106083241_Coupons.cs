using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnituresService.Data.Migrations
{
    /// <inheritdoc />
    public partial class Coupons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CouponCategoryId = table.Column<int>(type: "int", nullable: false),
                    CouponValuePercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coupons_Categories_CouponCategoryId",
                        column: x => x.CouponCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_CouponCategoryId",
                table: "Coupons",
                column: "CouponCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
