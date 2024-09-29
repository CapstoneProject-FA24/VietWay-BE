using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attraction_ManagerInfo_CreatedBy",
                table: "Attraction");

            migrationBuilder.DropForeignKey(
                name: "FK_AttractionType_ManagerInfo_CreatedBy",
                table: "AttractionType");

            migrationBuilder.DropForeignKey(
                name: "FK_Tour_StaffInfo_CreatorStaffId",
                table: "Tour");

            migrationBuilder.DropForeignKey(
                name: "FK_TourBooking_CustomerInfo_CustomerId",
                table: "TourBooking");

            migrationBuilder.DropForeignKey(
                name: "FK_TourTemplate_StaffInfo_CreatedBy",
                table: "TourTemplate");

            migrationBuilder.DropTable(
                name: "CustomerInfo");

            migrationBuilder.DropTable(
                name: "ManagerInfo");

            migrationBuilder.DropTable(
                name: "StaffInfo");

            migrationBuilder.DropTable(
                name: "AdminInfo");

            migrationBuilder.DropIndex(
                name: "IX_TourTemplate_CreatedBy",
                table: "TourTemplate");

            migrationBuilder.DropIndex(
                name: "IX_Tour_CreatorStaffId",
                table: "Tour");

            migrationBuilder.DropIndex(
                name: "IX_AttractionType_CreatedBy",
                table: "AttractionType");

            migrationBuilder.DropIndex(
                name: "IX_Attraction_CreatedBy",
                table: "Attraction");

            migrationBuilder.DropColumn(
                name: "CreatorStaffId",
                table: "Tour");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Image");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "AttractionType",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TourTemplate",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "CreatedBy",
                table: "TourCategory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TourCategory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TourCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tour",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SHA256",
                table: "Image",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CustomerFeedback",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Attraction",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Account",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    AdminId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.AdminId);
                    table.ForeignKey(
                        name: "FK_Admin_Account_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customer_Account_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "ProvinceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    ManagerId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.ManagerId);
                    table.ForeignKey(
                        name: "FK_Manager_Account_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StaffId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.StaffId);
                    table.ForeignKey(
                        name: "FK_Staff_Account_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ProvinceId",
                table: "Customer",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourBooking_Customer_CustomerId",
                table: "TourBooking",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourBooking_Customer_CustomerId",
                table: "TourBooking");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TourTemplate");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TourCategory");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TourCategory");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TourCategory");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tour");

            migrationBuilder.DropColumn(
                name: "SHA256",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CustomerFeedback");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Attraction");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "AttractionType",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<long>(
                name: "CreatorStaffId",
                table: "Tour",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Image",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Account",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "AdminInfo",
                columns: table => new
                {
                    AdminId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminInfo", x => x.AdminId);
                    table.ForeignKey(
                        name: "FK_AdminInfo_Account_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInfo",
                columns: table => new
                {
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInfo", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_CustomerInfo_Account_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerInfo_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "ProvinceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffInfo",
                columns: table => new
                {
                    StaffId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffInfo", x => x.StaffId);
                    table.ForeignKey(
                        name: "FK_StaffInfo_Account_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ManagerInfo",
                columns: table => new
                {
                    ManagerId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerInfo", x => x.ManagerId);
                    table.ForeignKey(
                        name: "FK_ManagerInfo_Account_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ManagerInfo_AdminInfo_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AdminInfo",
                        principalColumn: "AdminId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_CreatedBy",
                table: "TourTemplate",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tour_CreatorStaffId",
                table: "Tour",
                column: "CreatorStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionType_CreatedBy",
                table: "AttractionType",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_CreatedBy",
                table: "Attraction",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInfo_ProvinceId",
                table: "CustomerInfo",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerInfo_CreatedBy",
                table: "ManagerInfo",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Attraction_ManagerInfo_CreatedBy",
                table: "Attraction",
                column: "CreatedBy",
                principalTable: "ManagerInfo",
                principalColumn: "ManagerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AttractionType_ManagerInfo_CreatedBy",
                table: "AttractionType",
                column: "CreatedBy",
                principalTable: "ManagerInfo",
                principalColumn: "ManagerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tour_StaffInfo_CreatorStaffId",
                table: "Tour",
                column: "CreatorStaffId",
                principalTable: "StaffInfo",
                principalColumn: "StaffId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TourBooking_CustomerInfo_CustomerId",
                table: "TourBooking",
                column: "CustomerId",
                principalTable: "CustomerInfo",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TourTemplate_StaffInfo_CreatedBy",
                table: "TourTemplate",
                column: "CreatedBy",
                principalTable: "StaffInfo",
                principalColumn: "StaffId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
