using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWrongForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaPost_Attraction_EntityId",
                table: "SocialMediaPost");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaPost_Post_EntityId",
                table: "SocialMediaPost");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaPost_TourTemplate_EntityId",
                table: "SocialMediaPost");

            migrationBuilder.DropIndex(
                name: "IX_SocialMediaPost_EntityId",
                table: "SocialMediaPost");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaPost_EntityId",
                table: "SocialMediaPost",
                column: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaPost_Attraction_EntityId",
                table: "SocialMediaPost",
                column: "EntityId",
                principalTable: "Attraction",
                principalColumn: "AttractionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaPost_Post_EntityId",
                table: "SocialMediaPost",
                column: "EntityId",
                principalTable: "Post",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaPost_TourTemplate_EntityId",
                table: "SocialMediaPost",
                column: "EntityId",
                principalTable: "TourTemplate",
                principalColumn: "TourTemplateId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
