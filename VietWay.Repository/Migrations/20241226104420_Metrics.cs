using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Metrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocialMediaPost",
                columns: table => new
                {
                    SocialPostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Site = table.Column<int>(type: "int", nullable: false),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaPost", x => x.SocialPostId);
                    table.ForeignKey(
                        name: "FK_SocialMediaPost_Attraction_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Attraction",
                        principalColumn: "AttractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SocialMediaPost_Post_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Post",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SocialMediaPost_TourTemplate_EntityId",
                        column: x => x.EntityId,
                        principalTable: "TourTemplate",
                        principalColumn: "TourTemplateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacebookPostMetric",
                columns: table => new
                {
                    MetricId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SocialPostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostClickCount = table.Column<int>(type: "int", nullable: true),
                    ImpressionCount = table.Column<int>(type: "int", nullable: true),
                    LikeCount = table.Column<int>(type: "int", nullable: true),
                    LoveCount = table.Column<int>(type: "int", nullable: true),
                    WowCount = table.Column<int>(type: "int", nullable: true),
                    HahaCount = table.Column<int>(type: "int", nullable: true),
                    SorryCount = table.Column<int>(type: "int", nullable: true),
                    AngerCount = table.Column<int>(type: "int", nullable: true),
                    ShareCount = table.Column<int>(type: "int", nullable: true),
                    CommentCount = table.Column<int>(type: "int", nullable: true),
                    ReferralCount = table.Column<int>(type: "int", nullable: true),
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
                name: "TwitterPostMetric",
                columns: table => new
                {
                    MetricId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SocialPostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RetweetCount = table.Column<int>(type: "int", nullable: true),
                    ReplyCount = table.Column<int>(type: "int", nullable: true),
                    LikeCount = table.Column<int>(type: "int", nullable: true),
                    QuoteCount = table.Column<int>(type: "int", nullable: true),
                    BookmarkCount = table.Column<int>(type: "int", nullable: true),
                    ImpressionCount = table.Column<int>(type: "int", nullable: true),
                    ReferralCount = table.Column<int>(type: "int", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_FacebookPostMetric_SocialPostId",
                table: "FacebookPostMetric",
                column: "SocialPostId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaPost_EntityId",
                table: "SocialMediaPost",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterPostMetric_SocialPostId",
                table: "TwitterPostMetric",
                column: "SocialPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacebookPostMetric");

            migrationBuilder.DropTable(
                name: "TwitterPostMetric");

            migrationBuilder.DropTable(
                name: "SocialMediaPost");
        }
    }
}
