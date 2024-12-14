using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Transportation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Transportation",
                table: "TourTemplate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_StartingProvince",
                table: "TourTemplate",
                column: "StartingProvince");

            migrationBuilder.AddForeignKey(
                name: "FK_TourTemplate_Province_StartingProvince",
                table: "TourTemplate",
                column: "StartingProvince",
                principalTable: "Province",
                principalColumn: "ProvinceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourTemplate_Province_StartingProvince",
                table: "TourTemplate");

            migrationBuilder.DropIndex(
                name: "IX_TourTemplate_StartingProvince",
                table: "TourTemplate");

            migrationBuilder.DropColumn(
                name: "Transportation",
                table: "TourTemplate");
        }
    }
}
