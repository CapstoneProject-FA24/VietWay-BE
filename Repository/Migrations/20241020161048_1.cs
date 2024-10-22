using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_PostCategory_EventCategoryId",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "EventCategoryId",
                table: "Post",
                newName: "PostCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_EventCategoryId",
                table: "Post",
                newName: "IX_Post_PostCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_PostCategory_PostCategoryId",
                table: "Post",
                column: "PostCategoryId",
                principalTable: "PostCategory",
                principalColumn: "PostCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_PostCategory_PostCategoryId",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "PostCategoryId",
                table: "Post",
                newName: "EventCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_PostCategoryId",
                table: "Post",
                newName: "IX_Post_EventCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_PostCategory_EventCategoryId",
                table: "Post",
                column: "EventCategoryId",
                principalTable: "PostCategory",
                principalColumn: "PostCategoryId");
        }
    }
}
