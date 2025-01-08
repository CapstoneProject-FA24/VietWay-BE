using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSocialMediaPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "SocialMediaPost");

            migrationBuilder.AddColumn<string>(
                name: "AttractionId",
                table: "SocialMediaPost",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostId",
                table: "SocialMediaPost",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TourTemplateId",
                table: "SocialMediaPost",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaPost_Attraction_AttractionId",
                table: "SocialMediaPost",
                column: "AttractionId",
                principalTable: "Attraction",
                principalColumn: "AttractionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaPost_Post_PostId",
                table: "SocialMediaPost",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaPost_TourTemplate_TourTemplateId",
                table: "SocialMediaPost",
                column: "TourTemplateId",
                principalTable: "TourTemplate",
                principalColumn: "TourTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaPost_Attraction_AttractionId",
                table: "SocialMediaPost");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaPost_Post_PostId",
                table: "SocialMediaPost");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaPost_TourTemplate_TourTemplateId",
                table: "SocialMediaPost");

            migrationBuilder.DropIndex(
                name: "IX_SocialMediaPost_AttractionId",
                table: "SocialMediaPost");

            migrationBuilder.DropIndex(
                name: "IX_SocialMediaPost_PostId",
                table: "SocialMediaPost");

            migrationBuilder.DropIndex(
                name: "IX_SocialMediaPost_TourTemplateId",
                table: "SocialMediaPost");

            migrationBuilder.DropColumn(
                name: "AttractionId",
                table: "SocialMediaPost");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "SocialMediaPost");

            migrationBuilder.DropColumn(
                name: "TourTemplateId",
                table: "SocialMediaPost");

            migrationBuilder.AddColumn<string>(
                name: "EntityId",
                table: "SocialMediaPost",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
