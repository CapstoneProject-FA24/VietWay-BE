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
                    AccountId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
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
                    ImageId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ImageId);
                });

            migrationBuilder.CreateTable(
                name: "TourDuration",
                columns: table => new
                {
                    DurationId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DurationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumberOfDay = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourDuration", x => x.DurationId);
                });

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    AdminId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
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
                name: "Province",
                columns: table => new
                {
                    ProvinceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProvinceName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
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
                name: "Manager",
                columns: table => new
                {
                    ManagerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Manager_Admin_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Admin",
                        principalColumn: "AdminId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    ProvinceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
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
                name: "AttractionType",
                columns: table => new
                {
                    AttractionTypeId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionType", x => x.AttractionTypeId);
                    table.ForeignKey(
                        name: "FK_AttractionType_Manager_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Manager",
                        principalColumn: "ManagerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StaffId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Staff_Manager_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Manager",
                        principalColumn: "ManagerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TourCategory",
                columns: table => new
                {
                    TourCategoryId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourCategory", x => x.TourCategoryId);
                    table.ForeignKey(
                        name: "FK_TourCategory_Manager_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Manager",
                        principalColumn: "ManagerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attraction",
                columns: table => new
                {
                    AttractionId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProvinceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AttractionTypeId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GooglePlaceId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        name: "FK_Attraction_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "ProvinceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attraction_Staff_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TourTemplate",
                columns: table => new
                {
                    TourTemplateId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TourName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    TourCategoryId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Policy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTemplate", x => x.TourTemplateId);
                    table.ForeignKey(
                        name: "FK_TourTemplate_Staff_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourTemplate_TourCategory_TourCategoryId",
                        column: x => x.TourCategoryId,
                        principalTable: "TourCategory",
                        principalColumn: "TourCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourTemplate_TourDuration_DurationId",
                        column: x => x.DurationId,
                        principalTable: "TourDuration",
                        principalColumn: "DurationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttractionImage",
                columns: table => new
                {
                    AttractionId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
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
                name: "Tour",
                columns: table => new
                {
                    TourId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TourTemplateId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxParticipant = table.Column<int>(type: "int", nullable: false),
                    MinParticipant = table.Column<int>(type: "int", nullable: false),
                    CurrentParticipant = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tour", x => x.TourId);
                    table.ForeignKey(
                        name: "FK_Tour_Staff_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Staff",
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
                name: "TourTemplateImage",
                columns: table => new
                {
                    TourTemplateId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(20)", nullable: false)
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
                    TourTemplateId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    ProvinceId = table.Column<string>(type: "nvarchar(20)", nullable: false)
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
                name: "TourTemplateSchedule",
                columns: table => new
                {
                    TourTemplateId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DayNumber = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTemplateSchedule", x => new { x.TourTemplateId, x.DayNumber });
                    table.ForeignKey(
                        name: "FK_TourTemplateSchedule_TourTemplate_TourTemplateId",
                        column: x => x.TourTemplateId,
                        principalTable: "TourTemplate",
                        principalColumn: "TourTemplateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TourBooking",
                columns: table => new
                {
                    BookingId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TourId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NumberOfParticipants = table.Column<int>(type: "int", nullable: false),
                    ContactFullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    ContactPhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ContactAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourBooking", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_TourBooking_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
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
                name: "AttractionSchedule",
                columns: table => new
                {
                    TourTemplateId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DayNumber = table.Column<int>(type: "int", nullable: false),
                    AttractionId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionSchedule", x => new { x.TourTemplateId, x.DayNumber, x.AttractionId });
                    table.ForeignKey(
                        name: "FK_AttractionSchedule_Attraction_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attraction",
                        principalColumn: "AttractionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttractionSchedule_TourTemplateSchedule_TourTemplateId_DayNumber",
                        columns: x => new { x.TourTemplateId, x.DayNumber },
                        principalTable: "TourTemplateSchedule",
                        principalColumns: new[] { "TourTemplateId", "DayNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingPayment",
                columns: table => new
                {
                    PaymentId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BookingId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankTransactionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThirdPartyTransactionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
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
                name: "BookingTourParticipant",
                columns: table => new
                {
                    ParticipantId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TourBookingId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    HasAttended = table.Column<bool>(type: "bit", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingTourParticipant", x => x.ParticipantId);
                    table.ForeignKey(
                        name: "FK_BookingTourParticipant_TourBooking_TourBookingId",
                        column: x => x.TourBookingId,
                        principalTable: "TourBooking",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerFeedback",
                columns: table => new
                {
                    FeedbackId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BookingId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_AttractionTypeId",
                table: "Attraction",
                column: "AttractionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_CreatedBy",
                table: "Attraction",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_ProvinceId",
                table: "Attraction",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionImage_ImageId",
                table: "AttractionImage",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionSchedule_AttractionId",
                table: "AttractionSchedule",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionType_CreatedBy",
                table: "AttractionType",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayment_BookingId",
                table: "BookingPayment",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTourParticipant_TourBookingId",
                table: "BookingTourParticipant",
                column: "TourBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ProvinceId",
                table: "Customer",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFeedback_BookingId",
                table: "CustomerFeedback",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manager_CreatedBy",
                table: "Manager",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Province_ImageId",
                table: "Province",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_CreatedBy",
                table: "Staff",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tour_CreatedBy",
                table: "Tour",
                column: "CreatedBy");

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
                name: "IX_TourCategory_CreatedBy",
                table: "TourCategory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_CreatedBy",
                table: "TourTemplate",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_DurationId",
                table: "TourTemplate",
                column: "DurationId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionImage");

            migrationBuilder.DropTable(
                name: "AttractionSchedule");

            migrationBuilder.DropTable(
                name: "BookingPayment");

            migrationBuilder.DropTable(
                name: "BookingTourParticipant");

            migrationBuilder.DropTable(
                name: "CustomerFeedback");

            migrationBuilder.DropTable(
                name: "TourTemplateImage");

            migrationBuilder.DropTable(
                name: "TourTemplateProvince");

            migrationBuilder.DropTable(
                name: "Attraction");

            migrationBuilder.DropTable(
                name: "TourTemplateSchedule");

            migrationBuilder.DropTable(
                name: "TourBooking");

            migrationBuilder.DropTable(
                name: "AttractionType");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Tour");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "TourTemplate");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "TourCategory");

            migrationBuilder.DropTable(
                name: "TourDuration");

            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
