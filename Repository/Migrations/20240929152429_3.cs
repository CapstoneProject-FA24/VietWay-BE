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
            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_CreatedBy",
                table: "TourTemplate",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TourCategory_CreatedBy",
                table: "TourCategory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tour_CreatedBy",
                table: "Tour",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_CreatedBy",
                table: "Staff",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Manager_CreatedBy",
                table: "Manager",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionType_CreatedBy",
                table: "AttractionType",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_CreatedBy",
                table: "Attraction",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Attraction_Staff_CreatedBy",
                table: "Attraction",
                column: "CreatedBy",
                principalTable: "Staff",
                principalColumn: "StaffId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AttractionType_Manager_CreatedBy",
                table: "AttractionType",
                column: "CreatedBy",
                principalTable: "Manager",
                principalColumn: "ManagerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Manager_Admin_CreatedBy",
                table: "Manager",
                column: "CreatedBy",
                principalTable: "Admin",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Manager_CreatedBy",
                table: "Staff",
                column: "CreatedBy",
                principalTable: "Manager",
                principalColumn: "ManagerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tour_Staff_CreatedBy",
                table: "Tour",
                column: "CreatedBy",
                principalTable: "Staff",
                principalColumn: "StaffId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TourCategory_Manager_CreatedBy",
                table: "TourCategory",
                column: "CreatedBy",
                principalTable: "Manager",
                principalColumn: "ManagerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TourTemplate_Staff_CreatedBy",
                table: "TourTemplate",
                column: "CreatedBy",
                principalTable: "Staff",
                principalColumn: "StaffId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attraction_Staff_CreatedBy",
                table: "Attraction");

            migrationBuilder.DropForeignKey(
                name: "FK_AttractionType_Manager_CreatedBy",
                table: "AttractionType");

            migrationBuilder.DropForeignKey(
                name: "FK_Manager_Admin_CreatedBy",
                table: "Manager");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Manager_CreatedBy",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_Tour_Staff_CreatedBy",
                table: "Tour");

            migrationBuilder.DropForeignKey(
                name: "FK_TourCategory_Manager_CreatedBy",
                table: "TourCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_TourTemplate_Staff_CreatedBy",
                table: "TourTemplate");

            migrationBuilder.DropIndex(
                name: "IX_TourTemplate_CreatedBy",
                table: "TourTemplate");

            migrationBuilder.DropIndex(
                name: "IX_TourCategory_CreatedBy",
                table: "TourCategory");

            migrationBuilder.DropIndex(
                name: "IX_Tour_CreatedBy",
                table: "Tour");

            migrationBuilder.DropIndex(
                name: "IX_Staff_CreatedBy",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Manager_CreatedBy",
                table: "Manager");

            migrationBuilder.DropIndex(
                name: "IX_AttractionType_CreatedBy",
                table: "AttractionType");

            migrationBuilder.DropIndex(
                name: "IX_Attraction_CreatedBy",
                table: "Attraction");
        }
    }
}
