using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ReportCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    TwitterReferralCount = table.Column<int>(type: "int", nullable: false),
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
                    FacebookReactionCount = table.Column<int>(type: "int", nullable: false),
                    XRetweetCount = table.Column<int>(type: "int", nullable: false),
                    XReplyCount = table.Column<int>(type: "int", nullable: false),
                    XLikeCount = table.Column<int>(type: "int", nullable: false),
                    XQuoteCount = table.Column<int>(type: "int", nullable: false),
                    XBookmarkCount = table.Column<int>(type: "int", nullable: false),
                    XImpressionCount = table.Column<int>(type: "int", nullable: false),
                    FacebookScore = table.Column<double>(type: "float", nullable: false),
                    XScore = table.Column<double>(type: "float", nullable: false),
                    SiteScore = table.Column<double>(type: "float", nullable: false),
                    AverageScore = table.Column<double>(type: "float", nullable: false),
                    FacebookCTR = table.Column<double>(type: "float", nullable: false),
                    XCTR = table.Column<double>(type: "float", nullable: false)
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
                    FacebookReactionCount = table.Column<int>(type: "int", nullable: false),
                    XRetweetCount = table.Column<int>(type: "int", nullable: false),
                    XReplyCount = table.Column<int>(type: "int", nullable: false),
                    XLikeCount = table.Column<int>(type: "int", nullable: false),
                    XQuoteCount = table.Column<int>(type: "int", nullable: false),
                    XBookmarkCount = table.Column<int>(type: "int", nullable: false),
                    XImpressionCount = table.Column<int>(type: "int", nullable: false),
                    FacebookScore = table.Column<double>(type: "float", nullable: false),
                    TwitterScore = table.Column<double>(type: "float", nullable: false),
                    FacebookCTR = table.Column<double>(type: "float", nullable: false),
                    TwitterCTR = table.Column<double>(type: "float", nullable: false)
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
                name: "PostReport",
                columns: table => new
                {
                    ReportId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportPeriod = table.Column<int>(type: "int", nullable: false),
                    ProvinceId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PostCategoryId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SiteReferralCount = table.Column<int>(type: "int", nullable: false),
                    FacebookReferralCount = table.Column<int>(type: "int", nullable: false),
                    TwitterReferralCount = table.Column<int>(type: "int", nullable: false),
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
                    FacebookReactionCount = table.Column<int>(type: "int", nullable: false),
                    RetweetCount = table.Column<int>(type: "int", nullable: false),
                    ReplyCount = table.Column<int>(type: "int", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    QuoteCount = table.Column<int>(type: "int", nullable: false),
                    BookmarkCount = table.Column<int>(type: "int", nullable: false),
                    ImpressionCount = table.Column<int>(type: "int", nullable: false),
                    FacebookScore = table.Column<double>(type: "float", nullable: false),
                    TwitterScore = table.Column<double>(type: "float", nullable: false),
                    SiteScore = table.Column<double>(type: "float", nullable: false),
                    AverageScore = table.Column<double>(type: "float", nullable: false),
                    FacebookCTR = table.Column<double>(type: "float", nullable: false),
                    TwitterCTR = table.Column<double>(type: "float", nullable: false)
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
                    SiteReferralCount = table.Column<int>(type: "int", nullable: false),
                    FacebookReferralCount = table.Column<int>(type: "int", nullable: false),
                    TwitterReferralCount = table.Column<int>(type: "int", nullable: false),
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
                    FacebookReactionCount = table.Column<int>(type: "int", nullable: false),
                    RetweetCount = table.Column<int>(type: "int", nullable: false),
                    ReplyCount = table.Column<int>(type: "int", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    QuoteCount = table.Column<int>(type: "int", nullable: false),
                    BookmarkCount = table.Column<int>(type: "int", nullable: false),
                    ImpressionCount = table.Column<int>(type: "int", nullable: false),
                    FacebookScore = table.Column<double>(type: "float", nullable: false),
                    TwitterScore = table.Column<double>(type: "float", nullable: false),
                    SiteScore = table.Column<double>(type: "float", nullable: false),
                    AverageScore = table.Column<double>(type: "float", nullable: false),
                    FacebookCTR = table.Column<double>(type: "float", nullable: false),
                    TwitterCTR = table.Column<double>(type: "float", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_AttractionReport_AttractionCategoryId",
                table: "AttractionReport",
                column: "AttractionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionReport_ProvinceId",
                table: "AttractionReport",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_HashtagReport_HashtagId",
                table: "HashtagReport",
                column: "HashtagId");

            migrationBuilder.CreateIndex(
                name: "IX_PostReport_PostCategoryId",
                table: "PostReport",
                column: "PostCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PostReport_ProvinceId",
                table: "PostReport",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplateReport_ProvinceId",
                table: "TourTemplateReport",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplateReport_TourCategoryId",
                table: "TourTemplateReport",
                column: "TourCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionReport");

            migrationBuilder.DropTable(
                name: "HashtagReport");

            migrationBuilder.DropTable(
                name: "PostReport");

            migrationBuilder.DropTable(
                name: "TourTemplateReport");
        }
    }
}
