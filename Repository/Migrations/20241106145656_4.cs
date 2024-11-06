using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class _4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttractionReviewLike_AttractionReview_AttractionReviewId",
                table: "AttractionReviewLike");

            migrationBuilder.RenameColumn(
                name: "AttractionReviewId",
                table: "AttractionReviewLike",
                newName: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttractionReviewLike_AttractionReview_ReviewId",
                table: "AttractionReviewLike",
                column: "ReviewId",
                principalTable: "AttractionReview",
                principalColumn: "ReviewId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttractionReviewLike_AttractionReview_ReviewId",
                table: "AttractionReviewLike");

            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "AttractionReviewLike",
                newName: "AttractionReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttractionReviewLike_AttractionReview_AttractionReviewId",
                table: "AttractionReviewLike",
                column: "AttractionReviewId",
                principalTable: "AttractionReview",
                principalColumn: "ReviewId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
