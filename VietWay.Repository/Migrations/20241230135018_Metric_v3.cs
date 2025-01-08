using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Metric_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "New1StarRatingLikeCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "New2StarRatingLikeCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "New3StarRatingLikeCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "New4StarRatingLikeCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "New5StarRatingLikeCount",
                table: "TourTemplateMetric");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "New1StarRatingLikeCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New2StarRatingLikeCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New3StarRatingLikeCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New4StarRatingLikeCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New5StarRatingLikeCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);
        }
    }
}
