using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class EnablePostHashtag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SocialPostId",
                table: "SocialMediaPostHashtag",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialMediaPostHashtag",
                table: "SocialMediaPostHashtag",
                columns: new[] { "SocialPostId", "HashtagId" });

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaPostHashtag_HashtagId",
                table: "SocialMediaPostHashtag",
                column: "HashtagId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaPostHashtag_Hashtag_HashtagId",
                table: "SocialMediaPostHashtag",
                column: "HashtagId",
                principalTable: "Hashtag",
                principalColumn: "HashtagId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaPostHashtag_SocialMediaPost_SocialPostId",
                table: "SocialMediaPostHashtag",
                column: "SocialPostId",
                principalTable: "SocialMediaPost",
                principalColumn: "SocialPostId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaPostHashtag_Hashtag_HashtagId",
                table: "SocialMediaPostHashtag");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaPostHashtag_SocialMediaPost_SocialPostId",
                table: "SocialMediaPostHashtag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialMediaPostHashtag",
                table: "SocialMediaPostHashtag");

            migrationBuilder.DropIndex(
                name: "IX_SocialMediaPostHashtag_HashtagId",
                table: "SocialMediaPostHashtag");

            migrationBuilder.AlterColumn<string>(
                name: "SocialPostId",
                table: "SocialMediaPostHashtag",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
