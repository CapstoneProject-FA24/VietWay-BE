using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
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
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "AttractionType",
                columns: table => new
                {
                    AttractionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionType", x => x.AttractionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ImageId);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvinceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.ProvinceId);
                });

            migrationBuilder.CreateTable(
                name: "TourCategory",
                columns: table => new
                {
                    TourCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourCategory", x => x.TourCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ManagerInfo",
                columns: table => new
                {
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerInfo", x => x.ManagerId);
                    table.ForeignKey(
                        name: "FK_ManagerInfo_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyStaffInfo",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyStaffInfo", x => x.StaffId);
                    table.ForeignKey(
                        name: "FK_CompanyStaffInfo_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyStaffInfo_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInfo",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInfo", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_CustomerInfo_Account_AccountId",
                        column: x => x.AccountId,
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
                name: "Attraction",
                columns: table => new
                {
                    AttractionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    AttractionTypeId = table.Column<int>(type: "int", nullable: false),
                    AttractionStatus = table.Column<int>(type: "int", nullable: false),
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
                name: "TourGuideInfo",
                columns: table => new
                {
                    TourGuideId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourGuideInfo", x => x.TourGuideId);
                    table.ForeignKey(
                        name: "FK_TourGuideInfo_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourGuideInfo_CompanyStaffInfo_AddedBy",
                        column: x => x.AddedBy,
                        principalTable: "CompanyStaffInfo",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourGuideInfo_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TourTemplate",
                columns: table => new
                {
                    TourTemplateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TourCategoryId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Policy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Schedule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTemplate", x => x.TourTemplateId);
                    table.ForeignKey(
                        name: "FK_TourTemplate_CompanyStaffInfo_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "CompanyStaffInfo",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourTemplate_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourTemplate_TourCategory_TourCategoryId",
                        column: x => x.TourCategoryId,
                        principalTable: "TourCategory",
                        principalColumn: "TourCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttractionImage",
                columns: table => new
                {
                    AttractionId = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: false)
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
                    TourId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourTemplateId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxParticipant = table.Column<int>(type: "int", nullable: false),
                    MinParticipant = table.Column<int>(type: "int", nullable: false),
                    CurrentParticipant = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tour", x => x.TourId);
                    table.ForeignKey(
                        name: "FK_Tour_CompanyStaffInfo_CreatedBy",
                        column: x => x.CreatedBy,
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
                    TourTemplateId = table.Column<int>(type: "int", nullable: false),
                    AttractionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTemplateAttraction", x => new { x.TourTemplateId, x.AttractionId });
                    table.ForeignKey(
                        name: "FK_TourTemplateAttraction_Attraction_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attraction",
                        principalColumn: "AttractionId",
                        onDelete: ReferentialAction.Restrict);
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
                    TourTemplateId = table.Column<int>(type: "int", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: false)
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
                    TourTemplateId = table.Column<int>(type: "int", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
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
                name: "TourBooking",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
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
                name: "TourGuideAssignment",
                columns: table => new
                {
                    TourId = table.Column<int>(type: "int", nullable: false),
                    TourGuideId = table.Column<int>(type: "int", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourGuideAssignment", x => new { x.TourId, x.TourGuideId });
                    table.ForeignKey(
                        name: "FK_TourGuideAssignment_CompanyStaffInfo_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "CompanyStaffInfo",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourGuideAssignment_TourGuideInfo_TourGuideId",
                        column: x => x.TourGuideId,
                        principalTable: "TourGuideInfo",
                        principalColumn: "TourGuideId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TourGuideAssignment_Tour_TourId",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "TourId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingPayment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
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
                    FeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
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
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
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
                name: "IX_BookingPayment_BookingId",
                table: "BookingPayment",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStaffInfo_AccountId",
                table: "CompanyStaffInfo",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStaffInfo_CompanyId",
                table: "CompanyStaffInfo",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFeedback_BookingId",
                table: "CustomerFeedback",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInfo_AccountId",
                table: "CustomerInfo",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInfo_ProvinceId",
                table: "CustomerInfo",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerInfo_AccountId",
                table: "ManagerInfo",
                column: "AccountId");

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
                name: "IX_TourGuideAssignment_AssignedBy",
                table: "TourGuideAssignment",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideAssignment_TourGuideId",
                table: "TourGuideAssignment",
                column: "TourGuideId");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideInfo_AccountId",
                table: "TourGuideInfo",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideInfo_AddedBy",
                table: "TourGuideInfo",
                column: "AddedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideInfo_CompanyId",
                table: "TourGuideInfo",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_CompanyId",
                table: "TourTemplate",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_CreatedBy",
                table: "TourTemplate",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_TourCategoryId",
                table: "TourTemplate",
                column: "TourCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplateAttraction_AttractionId",
                table: "TourTemplateAttraction",
                column: "AttractionId");

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
                name: "CustomerFeedback");

            migrationBuilder.DropTable(
                name: "TourGuideAssignment");

            migrationBuilder.DropTable(
                name: "TourTemplateAttraction");

            migrationBuilder.DropTable(
                name: "TourTemplateImage");

            migrationBuilder.DropTable(
                name: "TourTemplateProvince");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "TourGuideInfo");

            migrationBuilder.DropTable(
                name: "Attraction");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "BookingPayment");

            migrationBuilder.DropTable(
                name: "AttractionType");

            migrationBuilder.DropTable(
                name: "ManagerInfo");

            migrationBuilder.DropTable(
                name: "TourBooking");

            migrationBuilder.DropTable(
                name: "CustomerInfo");

            migrationBuilder.DropTable(
                name: "Tour");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "TourTemplate");

            migrationBuilder.DropTable(
                name: "CompanyStaffInfo");

            migrationBuilder.DropTable(
                name: "TourCategory");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
