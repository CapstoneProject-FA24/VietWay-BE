using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourTemplate_StaffInfo_CreatorStaffId",
                table: "TourTemplate");

            migrationBuilder.DropIndex(
                name: "IX_TourTemplate_CreatorStaffId",
                table: "TourTemplate");

            migrationBuilder.DropColumn(
                name: "CreatorStaffId",
                table: "TourTemplate");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_CreatedBy",
                table: "TourTemplate",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_TourTemplate_StaffInfo_CreatedBy",
                table: "TourTemplate",
                column: "CreatedBy",
                principalTable: "StaffInfo",
                principalColumn: "StaffId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourTemplate_StaffInfo_CreatedBy",
                table: "TourTemplate");

            migrationBuilder.DropIndex(
                name: "IX_TourTemplate_CreatedBy",
                table: "TourTemplate");

            migrationBuilder.AddColumn<long>(
                name: "CreatorStaffId",
                table: "TourTemplate",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_CreatorStaffId",
                table: "TourTemplate",
                column: "CreatorStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourTemplate_StaffInfo_CreatorStaffId",
                table: "TourTemplate",
                column: "CreatorStaffId",
                principalTable: "StaffInfo",
                principalColumn: "StaffId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
