using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class renameFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "New1StarRatingCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "New2StarRatingCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "New3StarRatingCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "New4StarRatingCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "New5StarRatingCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "NewBookingCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "NewCancellationCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "NewFacebookReferralCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "NewViewCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "NewXReferralCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "NewFacebookReferralCount",
                table: "PostMetric");

            migrationBuilder.DropColumn(
                name: "NewSaveCount",
                table: "PostMetric");

            migrationBuilder.DropColumn(
                name: "NewViewCount",
                table: "PostMetric");

            migrationBuilder.DropColumn(
                name: "NewXReferralCount",
                table: "PostMetric");

            migrationBuilder.DropColumn(
                name: "New1StarRatingCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "New1StarRatingLikeCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "New2StarRatingCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "New2StarRatingLikeCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "New3StarRatingCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "New3StarRatingLikeCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "New4StarRatingCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "New4StarRatingLikeCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "New5StarRatingCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "New5StarRatingLikeCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "NewFacebookReferralCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "NewLikeCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "NewViewCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "NewXReferralCount",
                table: "AttractionMetric");

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "TwitterPostMetric",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RetweetCount",
                table: "TwitterPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReplyCount",
                table: "TwitterPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuoteCount",
                table: "TwitterPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LikeCount",
                table: "TwitterPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ImpressionCount",
                table: "TwitterPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookmarkCount",
                table: "TwitterPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "TourTemplateMetric",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookingCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CancellationCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FacebookReferralCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FiveStarRatingCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FourStarRatingCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OneStarRatingCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteReferralCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThreeStarRatingCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TwoStarRatingCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "XReferralCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "PostMetric",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacebookReferralCount",
                table: "PostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteReferralCount",
                table: "PostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteSaveCount",
                table: "PostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "XReferralCount",
                table: "PostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "WowCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SorryCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShareCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "FacebookPostMetric",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PostClickCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LoveCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LikeCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ImpressionCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HahaCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AngerCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "AttractionMetric",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacebookReferralCount",
                table: "AttractionMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FiveStarRatingCount",
                table: "AttractionMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FiveStarRatingLikeCount",
                table: "AttractionMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FourStarRatingCount",
                table: "AttractionMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FourStarRatingLikeCount",
                table: "AttractionMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OneStarRatingCount",
                table: "AttractionMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OneStarRatingLikeCount",
                table: "AttractionMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteLikeCount",
                table: "AttractionMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteReferralCount",
                table: "AttractionMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThreeStarRatingCount",
                table: "AttractionMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThreeStarRatingLikeCount",
                table: "AttractionMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TwoStarRatingCount",
                table: "AttractionMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TwoStarRatingLikeCount",
                table: "AttractionMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "XReferralCount",
                table: "AttractionMetric",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "CancellationCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "FacebookReferralCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "FiveStarRatingCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "FourStarRatingCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "OneStarRatingCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "SiteReferralCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "ThreeStarRatingCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "TwoStarRatingCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "XReferralCount",
                table: "TourTemplateMetric");

            migrationBuilder.DropColumn(
                name: "FacebookReferralCount",
                table: "PostMetric");

            migrationBuilder.DropColumn(
                name: "SiteReferralCount",
                table: "PostMetric");

            migrationBuilder.DropColumn(
                name: "SiteSaveCount",
                table: "PostMetric");

            migrationBuilder.DropColumn(
                name: "XReferralCount",
                table: "PostMetric");

            migrationBuilder.DropColumn(
                name: "FacebookReferralCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "FiveStarRatingCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "FiveStarRatingLikeCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "FourStarRatingCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "FourStarRatingLikeCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "OneStarRatingCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "OneStarRatingLikeCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "SiteLikeCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "SiteReferralCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "ThreeStarRatingCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "ThreeStarRatingLikeCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "TwoStarRatingCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "TwoStarRatingLikeCount",
                table: "AttractionMetric");

            migrationBuilder.DropColumn(
                name: "XReferralCount",
                table: "AttractionMetric");

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "TwitterPostMetric",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "RetweetCount",
                table: "TwitterPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ReplyCount",
                table: "TwitterPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "QuoteCount",
                table: "TwitterPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LikeCount",
                table: "TwitterPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ImpressionCount",
                table: "TwitterPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BookmarkCount",
                table: "TwitterPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "TourTemplateMetric",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "New1StarRatingCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New2StarRatingCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New3StarRatingCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New4StarRatingCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New5StarRatingCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewBookingCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewCancellationCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewFacebookReferralCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewViewCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewXReferralCount",
                table: "TourTemplateMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "PostMetric",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "NewFacebookReferralCount",
                table: "PostMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewSaveCount",
                table: "PostMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewViewCount",
                table: "PostMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewXReferralCount",
                table: "PostMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WowCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SorryCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ShareCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "FacebookPostMetric",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "PostClickCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LoveCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LikeCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ImpressionCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "HahaCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AngerCount",
                table: "FacebookPostMetric",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "AttractionMetric",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "New1StarRatingCount",
                table: "AttractionMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New1StarRatingLikeCount",
                table: "AttractionMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New2StarRatingCount",
                table: "AttractionMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New2StarRatingLikeCount",
                table: "AttractionMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New3StarRatingCount",
                table: "AttractionMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New3StarRatingLikeCount",
                table: "AttractionMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New4StarRatingCount",
                table: "AttractionMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New4StarRatingLikeCount",
                table: "AttractionMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New5StarRatingCount",
                table: "AttractionMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "New5StarRatingLikeCount",
                table: "AttractionMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewFacebookReferralCount",
                table: "AttractionMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewLikeCount",
                table: "AttractionMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewViewCount",
                table: "AttractionMetric",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewXReferralCount",
                table: "AttractionMetric",
                type: "int",
                nullable: true);
        }
    }
}
