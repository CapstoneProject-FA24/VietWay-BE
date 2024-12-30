using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Metrics_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferralCount",
                table: "TwitterPostMetric");

            migrationBuilder.DropColumn(
                name: "ReferralCount",
                table: "FacebookPostMetric");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PostLike",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AttractionReviewLike",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AttractionLike",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "AttractionMetric",
                columns: table => new
                {
                    MetricId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AttractionId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NewViewCount = table.Column<int>(type: "int", nullable: true),
                    NewLikeCount = table.Column<int>(type: "int", nullable: true),
                    NewFacebookReferralCount = table.Column<int>(type: "int", nullable: true),
                    NewXReferralCount = table.Column<int>(type: "int", nullable: true),
                    New5StarRatingCount = table.Column<int>(type: "int", nullable: true),
                    New5StarRatingLikeCount = table.Column<int>(type: "int", nullable: true),
                    New4StarRatingCount = table.Column<int>(type: "int", nullable: true),
                    New4StarRatingLikeCount = table.Column<int>(type: "int", nullable: true),
                    New3StarRatingCount = table.Column<int>(type: "int", nullable: true),
                    New3StarRatingLikeCount = table.Column<int>(type: "int", nullable: true),
                    New2StarRatingCount = table.Column<int>(type: "int", nullable: true),
                    New2StarRatingLikeCount = table.Column<int>(type: "int", nullable: true),
                    New1StarRatingCount = table.Column<int>(type: "int", nullable: true),
                    New1StarRatingLikeCount = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: true),
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
                name: "PostMetric",
                columns: table => new
                {
                    MetricId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NewViewCount = table.Column<int>(type: "int", nullable: true),
                    NewSaveCount = table.Column<int>(type: "int", nullable: true),
                    FacebookReferralCount = table.Column<int>(type: "int", nullable: true),
                    XReferralCount = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: true),
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
                name: "TourTemplateMetric",
                columns: table => new
                {
                    MetricId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TourTemplateId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NewViewCount = table.Column<int>(type: "int", nullable: true),
                    NewBookingCount = table.Column<int>(type: "int", nullable: true),
                    NewCancellationCount = table.Column<int>(type: "int", nullable: true),
                    NewFacebookReferralCount = table.Column<int>(type: "int", nullable: true),
                    NewXReferralCount = table.Column<int>(type: "int", nullable: true),
                    New5StarRatingCount = table.Column<int>(type: "int", nullable: true),
                    New5StarRatingLikeCount = table.Column<int>(type: "int", nullable: true),
                    New4StarRatingCount = table.Column<int>(type: "int", nullable: true),
                    New4StarRatingLikeCount = table.Column<int>(type: "int", nullable: true),
                    New3StarRatingCount = table.Column<int>(type: "int", nullable: true),
                    New3StarRatingLikeCount = table.Column<int>(type: "int", nullable: true),
                    New2StarRatingCount = table.Column<int>(type: "int", nullable: true),
                    New2StarRatingLikeCount = table.Column<int>(type: "int", nullable: true),
                    New1StarRatingCount = table.Column<int>(type: "int", nullable: true),
                    New1StarRatingLikeCount = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_AttractionMetric_AttractionId",
                table: "AttractionMetric",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_PostMetric_PostId",
                table: "PostMetric",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_TourTemplateMetric_TourTemplateId",
                table: "TourTemplateMetric",
                column: "TourTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionMetric");

            migrationBuilder.DropTable(
                name: "PostMetric");

            migrationBuilder.DropTable(
                name: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PostLike");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AttractionReviewLike");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AttractionLike");

            migrationBuilder.AddColumn<int>(
                name: "ReferralCount",
                table: "TwitterPostMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReferralCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: true);
        }
    }
}
