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
                name: "Hashtag",
                columns: table => new
                {
                    HashtagId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HashtagName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hashtag", x => x.HashtagId);
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
                name: "EntityHistory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ModifierRole = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityHistory_Account_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Account",
                        principalColumn: "AccountId");
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
                name: "HashtagReport",
                columns: table => new
                {
                    ReportId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportPeriod = table.Column<int>(type: "int", nullable: false),
                    HashtagId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FacebookReferralCount = table.Column<int>(type: "int", nullable: false),
                    XReferralCount = table.Column<int>(type: "int", nullable: false),
                    FacebookClickCount = table.Column<int>(type: "int", nullable: false),
                    FacebookImpressionCount = table.Column<int>(type: "int", nullable: false),
                    FacebookLikeCount = table.Column<int>(type: "int", nullable: false),
                    FacebookLoveCount = table.Column<int>(type: "int", nullable: false),
                    FacebookWowCount = table.Column<int>(type: "int", nullable: false),
                    FacebookHahaCount = table.Column<int>(type: "int", nullable: false),
                    FacebookSorryCount = table.Column<int>(type: "int", nullable: false),
                    FacebookAngerCount = table.Column<int>(type: "int", nullable: false),
                    FacebookShareCount = table.Column<int>(type: "int", nullable: false),
                    FacebookCommentCount = table.Column<int>(type: "int", nullable: false),
                    FacebookReactionCount = table.Column<int>(type: "int", nullable: false, computedColumnSql: "\r\n                    COALESCE([FacebookLikeCount], 0) + \r\n                    COALESCE([FacebookLoveCount], 0) + \r\n                    COALESCE([FacebookWowCount], 0) + \r\n                    COALESCE([FacebookHahaCount], 0) + \r\n                    COALESCE([FacebookSorryCount], 0) + \r\n                    COALESCE([FacebookAngerCount], 0)", stored: true),
                    XRetweetCount = table.Column<int>(type: "int", nullable: false),
                    XReplyCount = table.Column<int>(type: "int", nullable: false),
                    XLikeCount = table.Column<int>(type: "int", nullable: false),
                    XQuoteCount = table.Column<int>(type: "int", nullable: false),
                    XBookmarkCount = table.Column<int>(type: "int", nullable: false),
                    XImpressionCount = table.Column<int>(type: "int", nullable: false),
                    FacebookScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([FacebookClickCount], 0)*1 +\r\n                        COALESCE([FacebookImpressionCount], 0)*0.5 +\r\n                        COALESCE([FacebookLikeCount], 0)*1 + \r\n                        COALESCE([FacebookLoveCount], 0)*2 + \r\n                        COALESCE([FacebookWowCount], 0)*1.5 + \r\n                        COALESCE([FacebookHahaCount], 0)*1.5 + \r\n                        COALESCE([FacebookSorryCount], 0)*(-1) + \r\n                        COALESCE([FacebookAngerCount], 0)*(-2) + \r\n                        COALESCE([FacebookShareCount], 0)*3 + \r\n                        COALESCE([FacebookCommentCount], 0)*2\r\n                    AS decimal(18,2))", stored: true),
                    XScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([XRetweetCount], 0)*3 +\r\n                        COALESCE([XReplyCount], 0)*2 + \r\n                        COALESCE([XLikeCount], 0)*1.5 + \r\n                        COALESCE([XQuoteCount], 0)*3 + \r\n                        COALESCE([XBookmarkCount], 0)*2 + \r\n                        COALESCE([XImpressionCount], 0)*0.5\r\n                    AS decimal(18,2))", stored: true),
                    FacebookCTR = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        CASE\r\n                            WHEN ISNULL([FacebookImpressionCount],0) = 0 THEN 0\r\n                            ELSE COALESCE(CAST([FacebookReferralCount] AS decimal(18,2)) / CAST([FacebookImpressionCount] AS decimal(18,2)), 0)\r\n                        END \r\n                    AS decimal(18,2))", stored: true),
                    XCTR = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        CASE\r\n                            WHEN ISNULL([XImpressionCount],0) = 0 THEN 0\r\n                            ELSE COALESCE(CAST([XReferralCount] AS decimal(18,2)) / CAST([XImpressionCount] AS decimal(18,2)), 0)\r\n                        END \r\n                    AS decimal(18,2))", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HashtagReport", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_HashtagReport_Hashtag_HashtagId",
                        column: x => x.HashtagId,
                        principalTable: "Hashtag",
                        principalColumn: "HashtagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attraction",
                columns: table => new
                {
                    AttractionId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "AttractionReport",
                columns: table => new
                {
                    ReportId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportPeriod = table.Column<int>(type: "int", nullable: false),
                    ProvinceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AttractionCategoryId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SiteReferralCount = table.Column<int>(type: "int", nullable: false),
                    FacebookReferralCount = table.Column<int>(type: "int", nullable: false),
                    XReferralCount = table.Column<int>(type: "int", nullable: false),
                    SiteLikeCount = table.Column<int>(type: "int", nullable: false),
                    FiveStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    FiveStarRatingLikeCount = table.Column<int>(type: "int", nullable: false),
                    FourStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    FourStarRatingLikeCount = table.Column<int>(type: "int", nullable: false),
                    ThreeStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    ThreeStarRatingLikeCount = table.Column<int>(type: "int", nullable: false),
                    TwoStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    TwoStarRatingLikeCount = table.Column<int>(type: "int", nullable: false),
                    OneStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    OneStarRatingLikeCount = table.Column<int>(type: "int", nullable: false),
                    FacebookClickCount = table.Column<int>(type: "int", nullable: false),
                    FacebookImpressionCount = table.Column<int>(type: "int", nullable: false),
                    FacebookLikeCount = table.Column<int>(type: "int", nullable: false),
                    FacebookLoveCount = table.Column<int>(type: "int", nullable: false),
                    FacebookWowCount = table.Column<int>(type: "int", nullable: false),
                    FacebookHahaCount = table.Column<int>(type: "int", nullable: false),
                    FacebookSorryCount = table.Column<int>(type: "int", nullable: false),
                    FacebookAngerCount = table.Column<int>(type: "int", nullable: false),
                    FacebookShareCount = table.Column<int>(type: "int", nullable: false),
                    FacebookCommentCount = table.Column<int>(type: "int", nullable: false),
                    FacebookReactionCount = table.Column<int>(type: "int", nullable: false, computedColumnSql: "\r\n                    COALESCE([FacebookLikeCount], 0) + \r\n                    COALESCE([FacebookLoveCount], 0) + \r\n                    COALESCE([FacebookWowCount], 0) + \r\n                    COALESCE([FacebookHahaCount], 0) + \r\n                    COALESCE([FacebookSorryCount], 0) + \r\n                    COALESCE([FacebookAngerCount], 0)", stored: true),
                    XRetweetCount = table.Column<int>(type: "int", nullable: false),
                    XReplyCount = table.Column<int>(type: "int", nullable: false),
                    XLikeCount = table.Column<int>(type: "int", nullable: false),
                    XQuoteCount = table.Column<int>(type: "int", nullable: false),
                    XBookmarkCount = table.Column<int>(type: "int", nullable: false),
                    XImpressionCount = table.Column<int>(type: "int", nullable: false),
                    FacebookScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([FacebookClickCount], 0)*1 +\r\n                        COALESCE([FacebookImpressionCount], 0)*0.5 +\r\n                        COALESCE([FacebookLikeCount], 0)*1 + \r\n                        COALESCE([FacebookLoveCount], 0)*2 + \r\n                        COALESCE([FacebookWowCount], 0)*1.5 + \r\n                        COALESCE([FacebookHahaCount], 0)*1.5 + \r\n                        COALESCE([FacebookSorryCount], 0)*(-1) + \r\n                        COALESCE([FacebookAngerCount], 0)*(-2) + \r\n                        COALESCE([FacebookShareCount], 0)*3 + \r\n                        COALESCE([FacebookCommentCount], 0)*2\r\n                    AS decimal(18,2))", stored: true),
                    XScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([XRetweetCount], 0)*3 +\r\n                        COALESCE([XReplyCount], 0)*2 + \r\n                        COALESCE([XLikeCount], 0)*1.5 + \r\n                        COALESCE([XQuoteCount], 0)*3 + \r\n                        COALESCE([XBookmarkCount], 0)*2 + \r\n                        COALESCE([XImpressionCount], 0)*0.5\r\n                    AS decimal(18,2))", stored: true),
                    SiteScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([SiteReferralCount], 0)*2 +\r\n                        COALESCE([FacebookReferralCount], 0)*1 +\r\n                        COALESCE([XReferralCount], 0)*1 +\r\n                        COALESCE([SiteLikeCount], 0)*5 +\r\n                        COALESCE([FiveStarRatingCount], 0)*3 +\r\n                        COALESCE([FiveStarRatingLikeCount], 0)*3 +\r\n                        COALESCE([FourStarRatingCount], 0)*1 +\r\n                        COALESCE([FourStarRatingLikeCount], 0)*1 +\r\n                        COALESCE([ThreeStarRatingCount], 0)*0 +\r\n                        COALESCE([ThreeStarRatingLikeCount], 0)*0 +\r\n                        COALESCE([TwoStarRatingCount], 0)*(-1) +\r\n                        COALESCE([TwoStarRatingLikeCount], 0)*(-1) +\r\n                        COALESCE([OneStarRatingCount], 0)*(-3) +\r\n                        COALESCE([OneStarRatingLikeCount], 0)*(-3)\r\n                    AS decimal(18,2))", stored: true),
                    AverageScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        (CAST(\r\n                            COALESCE([FacebookClickCount], 0)*1 +\r\n                            COALESCE([FacebookImpressionCount], 0)*0.5 +\r\n                            COALESCE([FacebookLikeCount], 0)*1 + \r\n                            COALESCE([FacebookLoveCount], 0)*2 + \r\n                            COALESCE([FacebookWowCount], 0)*1.5 + \r\n                            COALESCE([FacebookHahaCount], 0)*1.5 + \r\n                            COALESCE([FacebookSorryCount], 0)*(-1) + \r\n                            COALESCE([FacebookAngerCount], 0)*(-2) + \r\n                            COALESCE([FacebookShareCount], 0)*3 + \r\n                            COALESCE([FacebookCommentCount], 0)*2\r\n                        AS decimal(18,2)) +\r\n                        CAST(\r\n                            COALESCE([SiteReferralCount], 0)*2 +\r\n                            COALESCE([FacebookReferralCount], 0)*1 +\r\n                            COALESCE([XReferralCount], 0)*1 +\r\n                            COALESCE([SiteLikeCount], 0)*5 +\r\n                            COALESCE([FiveStarRatingCount], 0)*3 +\r\n                            COALESCE([FiveStarRatingLikeCount], 0)*3 +\r\n                            COALESCE([FourStarRatingCount], 0)*1 +\r\n                            COALESCE([FourStarRatingLikeCount], 0)*1 +\r\n                            COALESCE([ThreeStarRatingCount], 0)*0 +\r\n                            COALESCE([ThreeStarRatingLikeCount], 0)*0 +\r\n                            COALESCE([TwoStarRatingCount], 0)*(-1) +\r\n                            COALESCE([TwoStarRatingLikeCount], 0)*(-1) +\r\n                            COALESCE([OneStarRatingCount], 0)*(-3) +\r\n                            COALESCE([OneStarRatingLikeCount], 0)*(-3)\r\n                        AS decimal(18,2)) +\r\n                        CAST(\r\n                            COALESCE([XRetweetCount], 0)*3 +\r\n                            COALESCE([XReplyCount], 0)*2 + \r\n                            COALESCE([XLikeCount], 0)*1.5 + \r\n                            COALESCE([XQuoteCount], 0)*3 + \r\n                            COALESCE([XBookmarkCount], 0)*2 + \r\n                            COALESCE([XImpressionCount], 0)*0.5\r\n                        AS decimal(18,2))) / 3.00\r\n                    AS decimal(18,2))", stored: true),
                    FacebookCTR = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n	                    CASE\r\n		                    WHEN ISNULL([FacebookImpressionCount],0) = 0 THEN 0\r\n		                    ELSE COALESCE(CAST([FacebookReferralCount] AS decimal(18,2)) / CAST([FacebookImpressionCount] AS decimal(18,2)), 0)\r\n	                    END \r\n                    AS decimal(18,2))", stored: true),
                    XCTR = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        CASE\r\n                            WHEN ISNULL([XImpressionCount],0) = 0 THEN 0\r\n                            ELSE COALESCE(CAST([XReferralCount] AS decimal(18,2)) / CAST([XImpressionCount] AS decimal(18,2)), 0)\r\n                        END \r\n                    AS decimal(18,2))", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionReport", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_AttractionReport_AttractionCategory_AttractionCategoryId",
                        column: x => x.AttractionCategoryId,
                        principalTable: "AttractionCategory",
                        principalColumn: "AttractionCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttractionReport_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "ProvinceId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "PostReport",
                columns: table => new
                {
                    ReportId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportPeriod = table.Column<int>(type: "int", nullable: false),
                    ProvinceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PostCategoryId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SiteReferralCount = table.Column<int>(type: "int", nullable: false),
                    FacebookReferralCount = table.Column<int>(type: "int", nullable: false),
                    XReferralCount = table.Column<int>(type: "int", nullable: false),
                    SiteLikeCount = table.Column<int>(type: "int", nullable: false),
                    FacebookClickCount = table.Column<int>(type: "int", nullable: false),
                    FacebookImpressionCount = table.Column<int>(type: "int", nullable: false),
                    FacebookLikeCount = table.Column<int>(type: "int", nullable: false),
                    FacebookLoveCount = table.Column<int>(type: "int", nullable: false),
                    FacebookWowCount = table.Column<int>(type: "int", nullable: false),
                    FacebookHahaCount = table.Column<int>(type: "int", nullable: false),
                    FacebookSorryCount = table.Column<int>(type: "int", nullable: false),
                    FacebookAngerCount = table.Column<int>(type: "int", nullable: false),
                    FacebookShareCount = table.Column<int>(type: "int", nullable: false),
                    FacebookCommentCount = table.Column<int>(type: "int", nullable: false),
                    FacebookReactionCount = table.Column<int>(type: "int", nullable: false, computedColumnSql: "\r\n                    COALESCE([FacebookLikeCount], 0) + \r\n                    COALESCE([FacebookLoveCount], 0) + \r\n                    COALESCE([FacebookWowCount], 0) + \r\n                    COALESCE([FacebookHahaCount], 0) + \r\n                    COALESCE([FacebookSorryCount], 0) + \r\n                    COALESCE([FacebookAngerCount], 0)", stored: true),
                    XRetweetCount = table.Column<int>(type: "int", nullable: false),
                    XReplyCount = table.Column<int>(type: "int", nullable: false),
                    XLikeCount = table.Column<int>(type: "int", nullable: false),
                    XQuoteCount = table.Column<int>(type: "int", nullable: false),
                    XBookmarkCount = table.Column<int>(type: "int", nullable: false),
                    XImpressionCount = table.Column<int>(type: "int", nullable: false),
                    FacebookScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([FacebookClickCount], 0)*1 +\r\n                        COALESCE([FacebookImpressionCount], 0)*0.5 +\r\n                        COALESCE([FacebookLikeCount], 0)*1 + \r\n                        COALESCE([FacebookLoveCount], 0)*2 + \r\n                        COALESCE([FacebookWowCount], 0)*1.5 + \r\n                        COALESCE([FacebookHahaCount], 0)*1.5 + \r\n                        COALESCE([FacebookSorryCount], 0)*(-1) + \r\n                        COALESCE([FacebookAngerCount], 0)*(-2) + \r\n                        COALESCE([FacebookShareCount], 0)*3 + \r\n                        COALESCE([FacebookCommentCount], 0)*2\r\n                    AS decimal(18,2))", stored: true),
                    XScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([XRetweetCount], 0)*3 +\r\n                        COALESCE([XReplyCount], 0)*2 + \r\n                        COALESCE([XLikeCount], 0)*1.5 + \r\n                        COALESCE([XQuoteCount], 0)*3 + \r\n                        COALESCE([XBookmarkCount], 0)*2 + \r\n                        COALESCE([XImpressionCount], 0)*0.5\r\n                    AS decimal(18,2))", stored: true),
                    SiteScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([SiteReferralCount], 0)*2 +\r\n                        COALESCE([SiteLikeCount], 0)*5 +\r\n                        COALESCE([FacebookReferralCount], 0)*1 +\r\n                        COALESCE([XReferralCount], 0)*1\r\n                    AS decimal(18,2))", stored: true),
                    AverageScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        (CAST(\r\n                            COALESCE([SiteReferralCount], 0)*2 +\r\n                            COALESCE([SiteLikeCount], 0)*5 +\r\n                            COALESCE([FacebookReferralCount], 0)*1 +\r\n                            COALESCE([XReferralCount], 0)*1\r\n                        AS decimal(18,2)) + \r\n                        CAST(\r\n                            COALESCE([FacebookClickCount], 0)*1 +\r\n                            COALESCE([FacebookImpressionCount], 0)*0.5 +\r\n                            COALESCE([FacebookLikeCount], 0)*1 + \r\n                            COALESCE([FacebookLoveCount], 0)*2 + \r\n                            COALESCE([FacebookWowCount], 0)*1.5 + \r\n                            COALESCE([FacebookHahaCount], 0)*1.5 + \r\n                            COALESCE([FacebookSorryCount], 0)*(-1) + \r\n                            COALESCE([FacebookAngerCount], 0)*(-2) + \r\n                            COALESCE([FacebookShareCount], 0)*3 + \r\n                            COALESCE([FacebookCommentCount], 0)*2\r\n                        AS decimal(18,2)) + \r\n                        CAST(\r\n                            COALESCE([XRetweetCount], 0)*3 +\r\n                            COALESCE([XReplyCount], 0)*2 + \r\n                            COALESCE([XLikeCount], 0)*1.5 + \r\n                            COALESCE([XQuoteCount], 0)*3 + \r\n                            COALESCE([XBookmarkCount], 0)*2 + \r\n                            COALESCE([XImpressionCount], 0)*0.5\r\n                        AS decimal(18,2))) / 3.00\r\n                    AS decimal(18,2))", stored: true),
                    FacebookCTR = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        CASE\r\n                            WHEN ISNULL([FacebookImpressionCount],0) = 0 THEN 0\r\n                            ELSE COALESCE(CAST([FacebookReferralCount] AS decimal(18,2)) / CAST([FacebookImpressionCount] AS decimal(18,2)), 0)\r\n                        END \r\n                    AS decimal(18,2))", stored: true),
                    XCTR = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        CASE\r\n                            WHEN ISNULL([XImpressionCount],0) = 0 THEN 0\r\n                            ELSE COALESCE(CAST([XReferralCount] AS decimal(18,2)) / CAST([XImpressionCount] AS decimal(18,2)), 0)\r\n                        END \r\n                    AS decimal(18,2))", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostReport", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_PostReport_PostCategory_PostCategoryId",
                        column: x => x.PostCategoryId,
                        principalTable: "PostCategory",
                        principalColumn: "PostCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostReport_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "ProvinceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourTemplateReport",
                columns: table => new
                {
                    ReportId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportPeriod = table.Column<int>(type: "int", nullable: false),
                    ProvinceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TourCategoryId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SiteReferralCount = table.Column<int>(type: "int", nullable: false),
                    FacebookReferralCount = table.Column<int>(type: "int", nullable: false),
                    XReferralCount = table.Column<int>(type: "int", nullable: false),
                    SiteLikeCount = table.Column<int>(type: "int", nullable: false),
                    BookingCount = table.Column<int>(type: "int", nullable: false),
                    CancellationCount = table.Column<int>(type: "int", nullable: false),
                    FiveStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    FourStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    ThreeStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    TwoStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    OneStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    FacebookClickCount = table.Column<int>(type: "int", nullable: false),
                    FacebookImpressionCount = table.Column<int>(type: "int", nullable: false),
                    FacebookLikeCount = table.Column<int>(type: "int", nullable: false),
                    FacebookLoveCount = table.Column<int>(type: "int", nullable: false),
                    FacebookWowCount = table.Column<int>(type: "int", nullable: false),
                    FacebookHahaCount = table.Column<int>(type: "int", nullable: false),
                    FacebookSorryCount = table.Column<int>(type: "int", nullable: false),
                    FacebookAngerCount = table.Column<int>(type: "int", nullable: false),
                    FacebookShareCount = table.Column<int>(type: "int", nullable: false),
                    FacebookCommentCount = table.Column<int>(type: "int", nullable: false),
                    FacebookReactionCount = table.Column<int>(type: "int", nullable: false, computedColumnSql: "\r\n                    COALESCE([FacebookLikeCount], 0) + \r\n                    COALESCE([FacebookLoveCount], 0) + \r\n                    COALESCE([FacebookWowCount], 0) + \r\n                    COALESCE([FacebookHahaCount], 0) + \r\n                    COALESCE([FacebookSorryCount], 0) + \r\n                    COALESCE([FacebookAngerCount], 0)", stored: true),
                    XRetweetCount = table.Column<int>(type: "int", nullable: false),
                    XReplyCount = table.Column<int>(type: "int", nullable: false),
                    XLikeCount = table.Column<int>(type: "int", nullable: false),
                    XQuoteCount = table.Column<int>(type: "int", nullable: false),
                    XBookmarkCount = table.Column<int>(type: "int", nullable: false),
                    XImpressionCount = table.Column<int>(type: "int", nullable: false),
                    FacebookScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([FacebookClickCount], 0)*1 +\r\n                        COALESCE([FacebookImpressionCount], 0)*0.5 +\r\n                        COALESCE([FacebookLikeCount], 0)*1 + \r\n                        COALESCE([FacebookLoveCount], 0)*2 + \r\n                        COALESCE([FacebookWowCount], 0)*1.5 + \r\n                        COALESCE([FacebookHahaCount], 0)*1.5 + \r\n                        COALESCE([FacebookSorryCount], 0)*(-1) + \r\n                        COALESCE([FacebookAngerCount], 0)*(-2) + \r\n                        COALESCE([FacebookShareCount], 0)*3 + \r\n                        COALESCE([FacebookCommentCount], 0)*2\r\n                    AS decimal(18,2))", stored: true),
                    XScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([XRetweetCount], 0)*3 +\r\n                        COALESCE([XReplyCount], 0)*2 + \r\n                        COALESCE([XLikeCount], 0)*1.5 + \r\n                        COALESCE([XQuoteCount], 0)*3 + \r\n                        COALESCE([XBookmarkCount], 0)*2 + \r\n                        COALESCE([XImpressionCount], 0)*0.5\r\n                    AS decimal(18,2))", stored: true),
                    SiteScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([SiteReferralCount], 0)*2 +\r\n                        COALESCE([BookingCount], 0)*8 +\r\n                        COALESCE([CancellationCount], 0)*(-4) +\r\n                        COALESCE([FacebookReferralCount], 0)*1 +\r\n                        COALESCE([XReferralCount], 0)*1 +\r\n                        COALESCE([FiveStarRatingCount], 0)*3 +\r\n                        COALESCE([FourStarRatingCount], 0)*1 +\r\n                        COALESCE([ThreeStarRatingCount], 0)*0 +\r\n                        COALESCE([TwoStarRatingCount], 0)*(-1) +\r\n                        COALESCE([OneStarRatingCount], 0)*(-3)\r\n                    AS decimal(18,2))", stored: true),
                    AverageScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        (CAST(\r\n                            COALESCE([SiteReferralCount], 0)*2 +\r\n                            COALESCE([BookingCount], 0)*8 +\r\n                            COALESCE([CancellationCount], 0)*(-4) +\r\n                            COALESCE([FacebookReferralCount], 0)*1 +\r\n                            COALESCE([XReferralCount], 0)*1 +\r\n                            COALESCE([FiveStarRatingCount], 0)*3 +\r\n                            COALESCE([FourStarRatingCount], 0)*1 +\r\n                            COALESCE([ThreeStarRatingCount], 0)*0 +\r\n                            COALESCE([TwoStarRatingCount], 0)*(-1) +\r\n                            COALESCE([OneStarRatingCount], 0)*(-3)\r\n                        AS decimal(18,2)) + \r\n                        CAST(\r\n                            COALESCE([FacebookClickCount], 0)*1 +\r\n                            COALESCE([FacebookImpressionCount], 0)*0.5 +\r\n                            COALESCE([FacebookLikeCount], 0)*1 + \r\n                            COALESCE([FacebookLoveCount], 0)*2 + \r\n                            COALESCE([FacebookWowCount], 0)*1.5 + \r\n                            COALESCE([FacebookHahaCount], 0)*1.5 + \r\n                            COALESCE([FacebookSorryCount], 0)*(-1) + \r\n                            COALESCE([FacebookAngerCount], 0)*(-2) + \r\n                            COALESCE([FacebookShareCount], 0)*3 + \r\n                            COALESCE([FacebookCommentCount], 0)*2\r\n                        AS decimal(18,2)) + \r\n                        CAST(\r\n                            COALESCE([XRetweetCount], 0)*3 +\r\n                            COALESCE([XReplyCount], 0)*2 + \r\n                            COALESCE([XLikeCount], 0)*1.5 + \r\n                            COALESCE([XQuoteCount], 0)*3 + \r\n                            COALESCE([XBookmarkCount], 0)*2 + \r\n                            COALESCE([XImpressionCount], 0)*0.5\r\n                        AS decimal(18,2))) / 3.00\r\n                    AS decimal(18,2))", stored: true),
                    FacebookCTR = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        CASE\r\n                            WHEN ISNULL([FacebookImpressionCount],0) = 0 THEN 0\r\n                            ELSE COALESCE(CAST([FacebookReferralCount] AS decimal(18,2)) / CAST([FacebookImpressionCount] AS decimal(18,2)), 0)\r\n                        END \r\n                    AS decimal(18,2))", stored: true),
                    XCTR = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        CASE\r\n                            WHEN ISNULL([XImpressionCount],0) = 0 THEN 0\r\n                            ELSE COALESCE(CAST([XReferralCount] AS decimal(18,2)) / CAST([XImpressionCount] AS decimal(18,2)), 0)\r\n                        END \r\n                    AS decimal(18,2))", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTemplateReport", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_TourTemplateReport_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "ProvinceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourTemplateReport_TourCategory_TourCategoryId",
                        column: x => x.TourCategoryId,
                        principalTable: "TourCategory",
                        principalColumn: "TourCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourTemplate",
                columns: table => new
                {
                    TourTemplateId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TourName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartingProvince = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    TourCategoryId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Transportation = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        name: "FK_TourTemplate_Province_StartingProvince",
                        column: x => x.StartingProvince,
                        principalTable: "Province",
                        principalColumn: "ProvinceId");
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
                name: "AttractionMetric",
                columns: table => new
                {
                    MetricId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AttractionId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SiteReferralCount = table.Column<int>(type: "int", nullable: false),
                    SiteLikeCount = table.Column<int>(type: "int", nullable: false),
                    FacebookReferralCount = table.Column<int>(type: "int", nullable: false),
                    XReferralCount = table.Column<int>(type: "int", nullable: false),
                    FiveStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    FiveStarRatingLikeCount = table.Column<int>(type: "int", nullable: false),
                    FourStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    FourStarRatingLikeCount = table.Column<int>(type: "int", nullable: false),
                    ThreeStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    ThreeStarRatingLikeCount = table.Column<int>(type: "int", nullable: false),
                    TwoStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    TwoStarRatingLikeCount = table.Column<int>(type: "int", nullable: false),
                    OneStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    OneStarRatingLikeCount = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([SiteReferralCount], 0)*2 + \r\n                        COALESCE([SiteLikeCount], 0)*5 + \r\n                        COALESCE([FacebookReferralCount], 0)*1 + \r\n                        COALESCE([XReferralCount], 0)*1 + \r\n                        COALESCE([FiveStarRatingCount], 0)*3 + \r\n                        COALESCE([FiveStarRatingLikeCount], 0)*3 + \r\n                        COALESCE([FourStarRatingCount], 0)*1 + \r\n                        COALESCE([FourStarRatingLikeCount], 0)*1 + \r\n                        COALESCE([ThreeStarRatingCount], 0)*0 + \r\n                        COALESCE([ThreeStarRatingLikeCount], 0)*0 + \r\n                        COALESCE([TwoStarRatingCount], 0)*(-1) + \r\n                        COALESCE([TwoStarRatingLikeCount], 0)*(-1) + \r\n                        COALESCE([OneStarRatingCount], 0)*(-3) + \r\n                        COALESCE([OneStarRatingLikeCount], 0)*(-3)\r\n                    AS decimal(18,2))", stored: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionMetric", x => x.MetricId);
                    table.ForeignKey(
                        name: "FK_AttractionMetric_Attraction_AttractionId",
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
                    CustomerId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    CustomerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "PostMetric",
                columns: table => new
                {
                    MetricId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SiteReferralCount = table.Column<int>(type: "int", nullable: false),
                    SiteSaveCount = table.Column<int>(type: "int", nullable: false),
                    FacebookReferralCount = table.Column<int>(type: "int", nullable: false),
                    XReferralCount = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([SiteReferralCount], 0)*2 + \r\n                        COALESCE([SiteSaveCount], 0)*5 + \r\n                        COALESCE([FacebookReferralCount], 0)*1 + \r\n                        COALESCE([XReferralCount], 0)*1\r\n                    AS decimal(18,2))", stored: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostMetric", x => x.MetricId);
                    table.ForeignKey(
                        name: "FK_PostMetric_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "PostId");
                });

            migrationBuilder.CreateTable(
                name: "SocialMediaPost",
                columns: table => new
                {
                    SocialPostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Site = table.Column<int>(type: "int", nullable: false),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    AttractionId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PostId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TourTemplateId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaPost", x => x.SocialPostId);
                    table.ForeignKey(
                        name: "FK_SocialMediaPost_Attraction_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attraction",
                        principalColumn: "AttractionId");
                    table.ForeignKey(
                        name: "FK_SocialMediaPost_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "PostId");
                    table.ForeignKey(
                        name: "FK_SocialMediaPost_TourTemplate_TourTemplateId",
                        column: x => x.TourTemplateId,
                        principalTable: "TourTemplate",
                        principalColumn: "TourTemplateId");
                });

            migrationBuilder.CreateTable(
                name: "Tour",
                columns: table => new
                {
                    TourId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TourTemplateId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartLocationPlaceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DefaultTouristPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RegisterOpenDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisterCloseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxParticipant = table.Column<int>(type: "int", nullable: false),
                    MinParticipant = table.Column<int>(type: "int", nullable: false),
                    CurrentParticipant = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepositPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                name: "TourTemplateMetric",
                columns: table => new
                {
                    MetricId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TourTemplateId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SiteReferralCount = table.Column<int>(type: "int", nullable: false),
                    BookingCount = table.Column<int>(type: "int", nullable: false),
                    CancellationCount = table.Column<int>(type: "int", nullable: false),
                    FacebookReferralCount = table.Column<int>(type: "int", nullable: false),
                    XReferralCount = table.Column<int>(type: "int", nullable: false),
                    FiveStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    FourStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    ThreeStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    TwoStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    OneStarRatingCount = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([SiteReferralCount], 0)*2 + \r\n                        COALESCE([BookingCount], 0)*8 + \r\n                        COALESCE([CancellationCount], 0)*(-4) + \r\n                        COALESCE([FacebookReferralCount], 0)*1 + \r\n                        COALESCE([XReferralCount], 0)*1 + \r\n                        COALESCE([FiveStarRatingCount], 0)*3 + \r\n                        COALESCE([FourStarRatingCount], 0)*1 + \r\n                        COALESCE([ThreeStarRatingCount], 0)*0 + \r\n                        COALESCE([TwoStarRatingCount], 0)*(-1) + \r\n                        COALESCE([OneStarRatingCount], 0)*(-3)\r\n                    AS decimal(18,2))", stored: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTemplateMetric", x => x.MetricId);
                    table.ForeignKey(
                        name: "FK_TourTemplateMetric_TourTemplate_TourTemplateId",
                        column: x => x.TourTemplateId,
                        principalTable: "TourTemplate",
                        principalColumn: "TourTemplateId");
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
                    CustomerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "FacebookPostMetric",
                columns: table => new
                {
                    MetricId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SocialPostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostClickCount = table.Column<int>(type: "int", nullable: false),
                    ImpressionCount = table.Column<int>(type: "int", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    LoveCount = table.Column<int>(type: "int", nullable: false),
                    WowCount = table.Column<int>(type: "int", nullable: false),
                    HahaCount = table.Column<int>(type: "int", nullable: false),
                    SorryCount = table.Column<int>(type: "int", nullable: false),
                    AngerCount = table.Column<int>(type: "int", nullable: false),
                    ShareCount = table.Column<int>(type: "int", nullable: false),
                    CommentCount = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([PostClickCount], 0)*1 +\r\n                        COALESCE([ImpressionCount], 0)*0.5 +\r\n                        COALESCE([LikeCount], 0)*1 + \r\n                        COALESCE([LoveCount], 0)*2 + \r\n                        COALESCE([WowCount], 0)*1.5 + \r\n                        COALESCE([HahaCount], 0)*1.5 + \r\n                        COALESCE([SorryCount], 0)*(-1) + \r\n                        COALESCE([AngerCount], 0)*(-2) + \r\n                        COALESCE([ShareCount], 0)*3 + \r\n                        COALESCE([CommentCount], 0)*2\r\n                    AS decimal(18,2))", stored: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacebookPostMetric", x => x.MetricId);
                    table.ForeignKey(
                        name: "FK_FacebookPostMetric_SocialMediaPost_SocialPostId",
                        column: x => x.SocialPostId,
                        principalTable: "SocialMediaPost",
                        principalColumn: "SocialPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialMediaPostHashtag",
                columns: table => new
                {
                    SocialPostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HashtagId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaPostHashtag", x => new { x.SocialPostId, x.HashtagId });
                    table.ForeignKey(
                        name: "FK_SocialMediaPostHashtag_Hashtag_HashtagId",
                        column: x => x.HashtagId,
                        principalTable: "Hashtag",
                        principalColumn: "HashtagId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SocialMediaPostHashtag_SocialMediaPost_SocialPostId",
                        column: x => x.SocialPostId,
                        principalTable: "SocialMediaPost",
                        principalColumn: "SocialPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TwitterPostMetric",
                columns: table => new
                {
                    MetricId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SocialPostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RetweetCount = table.Column<int>(type: "int", nullable: false),
                    ReplyCount = table.Column<int>(type: "int", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    QuoteCount = table.Column<int>(type: "int", nullable: false),
                    BookmarkCount = table.Column<int>(type: "int", nullable: false),
                    ImpressionCount = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "\r\n                    CAST(\r\n                        COALESCE([RetweetCount], 0)*3 +\r\n                        COALESCE([ReplyCount], 0)*2 + \r\n                        COALESCE([LikeCount], 0)*1.5 + \r\n                        COALESCE([QuoteCount], 0)*3 + \r\n                        COALESCE([BookmarkCount], 0)*2 + \r\n                        COALESCE([ImpressionCount], 0)*0.5\r\n                    AS decimal(18,2))", stored: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterPostMetric", x => x.MetricId);
                    table.ForeignKey(
                        name: "FK_TwitterPostMetric_SocialMediaPost_SocialPostId",
                        column: x => x.SocialPostId,
                        principalTable: "SocialMediaPost",
                        principalColumn: "SocialPostId",
                        onDelete: ReferentialAction.Cascade);
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
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                name: "BookingRefund",
                columns: table => new
                {
                    RefundId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BookingId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RefundStatus = table.Column<int>(type: "int", nullable: false),
                    RefundDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefundNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankTransactionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingRefund", x => x.RefundId);
                    table.ForeignKey(
                        name: "FK_BookingRefund_Booking_BookingId",
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
                    PIN = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
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
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "IX_AttractionMetric_AttractionId",
                table: "AttractionMetric",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionReport_AttractionCategoryId",
                table: "AttractionReport",
                column: "AttractionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionReport_ProvinceId",
                table: "AttractionReport",
                column: "ProvinceId");

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
                name: "IX_Booking_CustomerId",
                table: "Booking",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_TourId",
                table: "Booking",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayment_BookingId",
                table: "BookingPayment",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRefund_BookingId",
                table: "BookingRefund",
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
                name: "IX_EntityHistory_ModifiedBy",
                table: "EntityHistory",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FacebookPostMetric_SocialPostId",
                table: "FacebookPostMetric",
                column: "SocialPostId");

            migrationBuilder.CreateIndex(
                name: "IX_HashtagReport_HashtagId",
                table: "HashtagReport",
                column: "HashtagId");

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
                name: "IX_PostMetric_PostId",
                table: "PostMetric",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostReport_PostCategoryId",
                table: "PostReport",
                column: "PostCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PostReport_ProvinceId",
                table: "PostReport",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaPost_AttractionId",
                table: "SocialMediaPost",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaPost_PostId",
                table: "SocialMediaPost",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaPost_TourTemplateId",
                table: "SocialMediaPost",
                column: "TourTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaPostHashtag_HashtagId",
                table: "SocialMediaPostHashtag",
                column: "HashtagId");

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
                name: "IX_TourTemplate_StartingProvince",
                table: "TourTemplate",
                column: "StartingProvince");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplate_TourCategoryId",
                table: "TourTemplate",
                column: "TourCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplateImage_TourTemplateId",
                table: "TourTemplateImage",
                column: "TourTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplateMetric_TourTemplateId",
                table: "TourTemplateMetric",
                column: "TourTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplateProvince_ProvinceId",
                table: "TourTemplateProvince",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplateReport_ProvinceId",
                table: "TourTemplateReport",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplateReport_TourCategoryId",
                table: "TourTemplateReport",
                column: "TourCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterPostMetric_SocialPostId",
                table: "TwitterPostMetric",
                column: "SocialPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionImage");

            migrationBuilder.DropTable(
                name: "AttractionLike");

            migrationBuilder.DropTable(
                name: "AttractionMetric");

            migrationBuilder.DropTable(
                name: "AttractionReport");

            migrationBuilder.DropTable(
                name: "AttractionReviewLike");

            migrationBuilder.DropTable(
                name: "AttractionSchedule");

            migrationBuilder.DropTable(
                name: "BookingPayment");

            migrationBuilder.DropTable(
                name: "BookingRefund");

            migrationBuilder.DropTable(
                name: "BookingTourist");

            migrationBuilder.DropTable(
                name: "EntityStatusHistory");

            migrationBuilder.DropTable(
                name: "FacebookPostMetric");

            migrationBuilder.DropTable(
                name: "HashtagReport");

            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropTable(
                name: "PostLike");

            migrationBuilder.DropTable(
                name: "PostMetric");

            migrationBuilder.DropTable(
                name: "PostReport");

            migrationBuilder.DropTable(
                name: "SocialMediaPostHashtag");

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
                name: "TourTemplateMetric");

            migrationBuilder.DropTable(
                name: "TourTemplateProvince");

            migrationBuilder.DropTable(
                name: "TourTemplateReport");

            migrationBuilder.DropTable(
                name: "TwitterPostMetric");

            migrationBuilder.DropTable(
                name: "AttractionReview");

            migrationBuilder.DropTable(
                name: "TourTemplateSchedule");

            migrationBuilder.DropTable(
                name: "EntityHistory");

            migrationBuilder.DropTable(
                name: "Hashtag");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "SocialMediaPost");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Tour");

            migrationBuilder.DropTable(
                name: "Attraction");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "TourTemplate");

            migrationBuilder.DropTable(
                name: "AttractionCategory");

            migrationBuilder.DropTable(
                name: "PostCategory");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "TourCategory");

            migrationBuilder.DropTable(
                name: "TourDuration");
        }
    }
}
