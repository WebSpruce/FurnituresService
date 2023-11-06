using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnituresService.Data.Migrations
{
    /// <inheritdoc />
    public partial class CouponCodeForCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Coupons_CouponId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_CouponId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "CouponId",
                table: "Coupons");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CouponId",
                table: "Coupons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_CouponId",
                table: "Coupons",
                column: "CouponId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Coupons_CouponId",
                table: "Coupons",
                column: "CouponId",
                principalTable: "Coupons",
                principalColumn: "Id");
        }
    }
}
