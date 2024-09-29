using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ImageId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ImageId);
                });

            migrationBuilder.CreateTable(
                name: "TourCategory",
                columns: table => new
                {
                    TourCategoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourCategory", x => x.TourCategoryId);
                });

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
                name: "CompanyStaffInfo",
                columns: table => new
                {
                    StaffId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyStaffInfo", x => x.StaffId);
                    table.ForeignKey(
                        name: "FK_CompanyStaffInfo_Account_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvinceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.ProvinceId);
                    table.ForeignKey(
                        name: "FK_Province_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ManagerInfo",
                columns: table => new
                {
                    ManagerId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "TourTemplate",
                columns: table => new
                {
                    TourTemplateId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TourName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TourCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Policy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatorStaffId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTemplate", x => x.TourTemplateId);
                    table.ForeignKey(
                        name: "FK_TourTemplate_CompanyStaffInfo_CreatorStaffId",
                        column: x => x.CreatorStaffId,
                        principalTable: "CompanyStaffInfo",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourTemplate_TourCategory_TourCategoryId",
                        column: x => x.TourCategoryId,
                        principalTable: "TourCategory",
                        principalColumn: "TourCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInfo",
                columns: table => new
                {
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
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
                name: "AttractionType",
                columns: table => new
                {
                    AttractionTypeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionType", x => x.AttractionTypeId);
                    table.ForeignKey(
                        name: "FK_AttractionType_ManagerInfo_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "ManagerInfo",
                        principalColumn: "ManagerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tour",
                columns: table => new
                {
                    TourId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxParticipant = table.Column<int>(type: "int", nullable: false),
                    MinParticipant = table.Column<int>(type: "int", nullable: false),
                    CurrentParticipant = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatorStaffId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tour", x => x.TourId);
                    table.ForeignKey(
                        name: "FK_Tour_CompanyStaffInfo_CreatorStaffId",
                        column: x => x.CreatorStaffId,
                        principalTable: "CompanyStaffInfo",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tour_TourTemplate_TourTemplateId",
                        column: x => x.TourTemplateId,
                        principalTable: "TourTemplate",
                        principalColumn: "TourTemplateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TourTemplateAttraction",
                columns: table => new
                {
                    TourTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    DayNumber = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTemplateAttraction", x => new { x.TourTemplateId, x.DayNumber });
                    table.ForeignKey(
                        name: "FK_TourTemplateAttraction_TourTemplate_TourTemplateId",
                        column: x => x.TourTemplateId,
                        principalTable: "TourTemplate",
                        principalColumn: "TourTemplateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TourTemplateImage",
                columns: table => new
                {
                    TourTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    ImageId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTemplateImage", x => new { x.TourTemplateId, x.ImageId });
                    table.ForeignKey(
                        name: "FK_TourTemplateImage_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourTemplateImage_TourTemplate_TourTemplateId",
                        column: x => x.TourTemplateId,
                        principalTable: "TourTemplate",
                        principalColumn: "TourTemplateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TourTemplateProvince",
                columns: table => new
                {
                    TourTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTemplateProvince", x => new { x.TourTemplateId, x.ProvinceId });
                    table.ForeignKey(
                        name: "FK_TourTemplateProvince_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "ProvinceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourTemplateProvince_TourTemplate_TourTemplateId",
                        column: x => x.TourTemplateId,
                        principalTable: "TourTemplate",
                        principalColumn: "TourTemplateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attraction",
                columns: table => new
                {
                    AttractionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    AttractionTypeId = table.Column<long>(type: "bigint", nullable: false),
                    GoogleMapUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attraction", x => x.AttractionId);
                    table.ForeignKey(
                        name: "FK_Attraction_AttractionType_AttractionTypeId",
                        column: x => x.AttractionTypeId,
                        principalTable: "AttractionType",
                        principalColumn: "AttractionTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attraction_ManagerInfo_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "ManagerInfo",
                        principalColumn: "ManagerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TourBooking",
                columns: table => new
                {
                    BookingId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourBooking", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_TourBooking_CustomerInfo_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "CustomerInfo",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourBooking_Tour_TourId",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "TourId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttractionImage",
                columns: table => new
                {
                    AttractionId = table.Column<long>(type: "bigint", nullable: false),
                    ImageId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionImage", x => new { x.AttractionId, x.ImageId });
                    table.ForeignKey(
                        name: "FK_AttractionImage_Attraction_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attraction",
                        principalColumn: "AttractionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttractionImage_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttractionSchedule",
                columns: table => new
                {
                    DayNumber = table.Column<int>(type: "int", nullable: false),
                    AttractionId = table.Column<long>(type: "bigint", nullable: false),
                    TourTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    TourTemplateScheduleDayNumber = table.Column<int>(type: "int", nullable: true),
                    TourTemplateScheduleTourTemplateId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionSchedule", x => new { x.AttractionId, x.DayNumber });
                    table.ForeignKey(
                        name: "FK_AttractionSchedule_Attraction_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attraction",
                        principalColumn: "AttractionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttractionSchedule_TourTemplateAttraction_TourTemplateId_DayNumber",
                        columns: x => new { x.TourTemplateId, x.DayNumber },
                        principalTable: "TourTemplateAttraction",
                        principalColumns: new[] { "TourTemplateId", "DayNumber" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttractionSchedule_TourTemplateAttraction_TourTemplateScheduleTourTemplateId_TourTemplateScheduleDayNumber",
                        columns: x => new { x.TourTemplateScheduleTourTemplateId, x.TourTemplateScheduleDayNumber },
                        principalTable: "TourTemplateAttraction",
                        principalColumns: new[] { "TourTemplateId", "DayNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingPayment",
                columns: table => new
                {
                    PaymentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPayment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_BookingPayment_TourBooking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "TourBooking",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerFeedback",
                columns: table => new
                {
                    FeedbackId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerFeedback", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK_CustomerFeedback_TourBooking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "TourBooking",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankTransactionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThirdPartyTransactionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_BookingPayment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "BookingPayment",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_AttractionTypeId",
                table: "Attraction",
                column: "AttractionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_CreatedBy",
                table: "Attraction",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionImage_ImageId",
                table: "AttractionImage",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionSchedule_TourTemplateId_DayNumber",
                table: "AttractionSchedule",
                columns: new[] { "TourTemplateId", "DayNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_AttractionSchedule_TourTemplateScheduleTourTemplateId_TourTemplateScheduleDayNumber",
                table: "AttractionSchedule",
                columns: new[] { "TourTemplateScheduleTourTemplateId", "TourTemplateScheduleDayNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_AttractionType_CreatedBy",
                table: "AttractionType",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayment_BookingId",
                table: "BookingPayment",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFeedback_BookingId",
                table: "CustomerFeedback",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInfo_ProvinceId",
                table: "CustomerInfo",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerInfo_CreatedBy",
                table: "ManagerInfo",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Province_ImageId",
                table: "Province",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Tour_CreatorStaffId",
                table: "Tour",
                column: "CreatorStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Tour_TourTemplateId",
                table: "Tour",
                column: "TourTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TourBooking_CustomerId",
                table: "TourBooking",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TourBooking_TourId",
                table: "TourBooking",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_CreatorStaffId",
                table: "TourTemplate",
                column: "CreatorStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_TourCategoryId",
                table: "TourTemplate",
                column: "TourCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplateImage_ImageId",
                table: "TourTemplateImage",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplateProvince_ProvinceId",
                table: "TourTemplateProvince",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PaymentId",
                table: "Transaction",
                column: "PaymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionImage");

            migrationBuilder.DropTable(
                name: "AttractionSchedule");

            migrationBuilder.DropTable(
                name: "CustomerFeedback");

            migrationBuilder.DropTable(
                name: "TourTemplateImage");

            migrationBuilder.DropTable(
                name: "TourTemplateProvince");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Attraction");

            migrationBuilder.DropTable(
                name: "TourTemplateAttraction");

            migrationBuilder.DropTable(
                name: "BookingPayment");

            migrationBuilder.DropTable(
                name: "AttractionType");

            migrationBuilder.DropTable(
                name: "TourBooking");

            migrationBuilder.DropTable(
                name: "ManagerInfo");

            migrationBuilder.DropTable(
                name: "CustomerInfo");

            migrationBuilder.DropTable(
                name: "Tour");

            migrationBuilder.DropTable(
                name: "AdminInfo");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "TourTemplate");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "CompanyStaffInfo");

            migrationBuilder.DropTable(
                name: "TourCategory");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
