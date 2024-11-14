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
                name: "AttractionCategory",
                columns: table => new
                {
                    AttractionCategoryId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionCategory", x => x.AttractionCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "EntityHistory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ModifierRole = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostCategory",
                columns: table => new
                {
                    PostCategoryId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategory", x => x.PostCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    ProvinceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.ProvinceId);
                });

            migrationBuilder.CreateTable(
                name: "TourCategory",
                columns: table => new
                {
                    TourCategoryId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourCategory", x => x.TourCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "TourDuration",
                columns: table => new
                {
                    DurationId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DurationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumberOfDay = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourDuration", x => x.DurationId);
                });

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    ManagerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.ManagerId);
                    table.ForeignKey(
                        name: "FK_Manager_Account_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StaffId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.StaffId);
                    table.ForeignKey(
                        name: "FK_Staff_Account_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityStatusHistory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OldStatus = table.Column<int>(type: "int", nullable: false),
                    NewStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityStatusHistory_EntityHistory_Id",
                        column: x => x.Id,
                        principalTable: "EntityHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attraction",
                columns: table => new
                {
                    AttractionId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ContactInfo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProvinceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AttractionCategoryId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GooglePlaceId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attraction", x => x.AttractionId);
                    table.ForeignKey(
                        name: "FK_Attraction_AttractionCategory_AttractionCategoryId",
                        column: x => x.AttractionCategoryId,
                        principalTable: "AttractionCategory",
                        principalColumn: "AttractionCategoryId");
                    table.ForeignKey(
                        name: "FK_Attraction_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "ProvinceId");
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customer_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "ProvinceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    PostId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCategoryId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    ProvinceId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    XTweetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacebookPostId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Post_PostCategory_PostCategoryId",
                        column: x => x.PostCategoryId,
                        principalTable: "PostCategory",
                        principalColumn: "PostCategoryId");
                    table.ForeignKey(
                        name: "FK_Post_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "ProvinceId");
                });

            migrationBuilder.CreateTable(
                name: "TourTemplate",
                columns: table => new
                {
                    TourTemplateId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TourName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    TourCategoryId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MinPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTemplate", x => x.TourTemplateId);
                    table.ForeignKey(
                        name: "FK_TourTemplate_TourCategory_TourCategoryId",
                        column: x => x.TourCategoryId,
                        principalTable: "TourCategory",
                        principalColumn: "TourCategoryId");
                    table.ForeignKey(
                        name: "FK_TourTemplate_TourDuration_DurationId",
                        column: x => x.DurationId,
                        principalTable: "TourDuration",
                        principalColumn: "DurationId");
                });

            migrationBuilder.CreateTable(
                name: "AttractionImage",
                columns: table => new
                {
                    ImageId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AttractionId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionImage", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_AttractionImage_Attraction_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attraction",
                        principalColumn: "AttractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttractionLike",
                columns: table => new
                {
                    AttractionId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionLike", x => new { x.AttractionId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_AttractionLike_Attraction_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attraction",
                        principalColumn: "AttractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttractionLike_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttractionReview",
                columns: table => new
                {
                    ReviewId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AttractionId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionReview", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_AttractionReview_Attraction_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attraction",
                        principalColumn: "AttractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttractionReview_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostLike",
                columns: table => new
                {
                    PostId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLike", x => new { x.PostId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_PostLike_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostLike_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tour",
                columns: table => new
                {
                    TourId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TourTemplateId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DefaultTouristPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RegisterOpenDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegisterCloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaxParticipant = table.Column<int>(type: "int", nullable: true),
                    MinParticipant = table.Column<int>(type: "int", nullable: true),
                    CurrentParticipant = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tour", x => x.TourId);
                    table.ForeignKey(
                        name: "FK_Tour_TourTemplate_TourTemplateId",
                        column: x => x.TourTemplateId,
                        principalTable: "TourTemplate",
                        principalColumn: "TourTemplateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourTemplateImage",
                columns: table => new
                {
                    ImageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TourTemplateId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTemplateImage", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_TourTemplateImage_TourTemplate_TourTemplateId",
                        column: x => x.TourTemplateId,
                        principalTable: "TourTemplate",
                        principalColumn: "TourTemplateId",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourTemplateProvince_TourTemplate_TourTemplateId",
                        column: x => x.TourTemplateId,
                        principalTable: "TourTemplate",
                        principalColumn: "TourTemplateId",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttractionReviewLike",
                columns: table => new
                {
                    ReviewId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionReviewLike", x => new { x.ReviewId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_AttractionReviewLike_AttractionReview_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "AttractionReview",
                        principalColumn: "ReviewId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttractionReviewLike_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
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
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Booking_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Tour_TourId",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "TourId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourPrice",
                columns: table => new
                {
                    PriceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TourId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AgeFrom = table.Column<int>(type: "int", nullable: false),
                    AgeTo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPrice", x => x.PriceId);
                    table.ForeignKey(
                        name: "FK_TourPrice_Tour_TourId",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "TourId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourRefundPolicy",
                columns: table => new
                {
                    TourRefundPolicyId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TourId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CancelBefore = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefundPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourRefundPolicy", x => x.TourRefundPolicyId);
                    table.ForeignKey(
                        name: "FK_TourRefundPolicy_Tour_TourId",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "TourId",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttractionSchedule_TourTemplateSchedule_TourTemplateId_DayNumber",
                        columns: x => new { x.TourTemplateId, x.DayNumber },
                        principalTable: "TourTemplateSchedule",
                        principalColumns: new[] { "TourTemplateId", "DayNumber" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingPayment",
                columns: table => new
                {
                    PaymentId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BookingId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                        name: "FK_BookingPayment_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingTourist",
                columns: table => new
                {
                    TouristId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BookingId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasAttended = table.Column<bool>(type: "bit", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingTourist", x => x.TouristId);
                    table.ForeignKey(
                        name: "FK_BookingTourist_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourReview",
                columns: table => new
                {
                    ReviewId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BookingId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourReview", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_TourReview_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Email",
                table: "Account",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Account_PhoneNumber",
                table: "Account",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_AttractionCategoryId",
                table: "Attraction",
                column: "AttractionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_ProvinceId",
                table: "Attraction",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionImage_AttractionId",
                table: "AttractionImage",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionLike_CustomerId",
                table: "AttractionLike",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionReview_AttractionId_CustomerId",
                table: "AttractionReview",
                columns: new[] { "AttractionId", "CustomerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttractionReview_CustomerId",
                table: "AttractionReview",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionReviewLike_CustomerId",
                table: "AttractionReviewLike",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionSchedule_AttractionId",
                table: "AttractionSchedule",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CustomerId_TourId",
                table: "Booking",
                columns: new[] { "CustomerId", "TourId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_TourId",
                table: "Booking",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayment_BookingId",
                table: "BookingPayment",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTourist_BookingId",
                table: "BookingTourist",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ProvinceId",
                table: "Customer",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_PostCategoryId",
                table: "Post",
                column: "PostCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_ProvinceId",
                table: "Post",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLike_CustomerId",
                table: "PostLike",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tour_TourTemplateId",
                table: "Tour",
                column: "TourTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPrice_TourId",
                table: "TourPrice",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_TourRefundPolicy_TourId",
                table: "TourRefundPolicy",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_TourReview_BookingId",
                table: "TourReview",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_DurationId",
                table: "TourTemplate",
                column: "DurationId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_TourCategoryId",
                table: "TourTemplate",
                column: "TourCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplateImage_TourTemplateId",
                table: "TourTemplateImage",
                column: "TourTemplateId");

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
                name: "AttractionLike");

            migrationBuilder.DropTable(
                name: "AttractionReviewLike");

            migrationBuilder.DropTable(
                name: "AttractionSchedule");

            migrationBuilder.DropTable(
                name: "BookingPayment");

            migrationBuilder.DropTable(
                name: "BookingTourist");

            migrationBuilder.DropTable(
                name: "EntityStatusHistory");

            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropTable(
                name: "PostLike");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "TourPrice");

            migrationBuilder.DropTable(
                name: "TourRefundPolicy");

            migrationBuilder.DropTable(
                name: "TourReview");

            migrationBuilder.DropTable(
                name: "TourTemplateImage");

            migrationBuilder.DropTable(
                name: "TourTemplateProvince");

            migrationBuilder.DropTable(
                name: "AttractionReview");

            migrationBuilder.DropTable(
                name: "TourTemplateSchedule");

            migrationBuilder.DropTable(
                name: "EntityHistory");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Attraction");

            migrationBuilder.DropTable(
                name: "PostCategory");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Tour");

            migrationBuilder.DropTable(
                name: "AttractionCategory");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "TourTemplate");

            migrationBuilder.DropTable(
                name: "TourCategory");

            migrationBuilder.DropTable(
                name: "TourDuration");
        }
    }
}
