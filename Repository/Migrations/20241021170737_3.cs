using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProvinceId",
                table: "TourTemplate",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_ProvinceId",
                table: "TourTemplate",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourTemplate_Province_ProvinceId",
                table: "TourTemplate",
                column: "ProvinceId",
                principalTable: "Province",
                principalColumn: "ProvinceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourTemplate_Province_ProvinceId",
                table: "TourTemplate");

            migrationBuilder.DropIndex(
                name: "IX_TourTemplate_ProvinceId",
                table: "TourTemplate");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "TourTemplate");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Event");
        }
    }
}
