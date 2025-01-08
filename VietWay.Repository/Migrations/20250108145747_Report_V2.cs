using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Report_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TwitterScore",
                table: "TourTemplateReport",
                newName: "XScore");

            migrationBuilder.RenameColumn(
                name: "TwitterReferralCount",
                table: "TourTemplateReport",
                newName: "XRetweetCount");

            migrationBuilder.RenameColumn(
                name: "TwitterCTR",
                table: "TourTemplateReport",
                newName: "XCTR");

            migrationBuilder.RenameColumn(
                name: "RetweetCount",
                table: "TourTemplateReport",
                newName: "XReplyCount");

            migrationBuilder.RenameColumn(
                name: "ReplyCount",
                table: "TourTemplateReport",
                newName: "XReferralCount");

            migrationBuilder.RenameColumn(
                name: "QuoteCount",
                table: "TourTemplateReport",
                newName: "XQuoteCount");

            migrationBuilder.RenameColumn(
                name: "LikeCount",
                table: "TourTemplateReport",
                newName: "XLikeCount");

            migrationBuilder.RenameColumn(
                name: "ImpressionCount",
                table: "TourTemplateReport",
                newName: "XImpressionCount");

            migrationBuilder.RenameColumn(
                name: "BookmarkCount",
                table: "TourTemplateReport",
                newName: "XBookmarkCount");

            migrationBuilder.RenameColumn(
                name: "TwitterScore",
                table: "PostReport",
                newName: "XScore");

            migrationBuilder.RenameColumn(
                name: "TwitterReferralCount",
                table: "PostReport",
                newName: "XRetweetCount");

            migrationBuilder.RenameColumn(
                name: "TwitterCTR",
                table: "PostReport",
                newName: "XCTR");

            migrationBuilder.RenameColumn(
                name: "RetweetCount",
                table: "PostReport",
                newName: "XReplyCount");

            migrationBuilder.RenameColumn(
                name: "ReplyCount",
                table: "PostReport",
                newName: "XReferralCount");

            migrationBuilder.RenameColumn(
                name: "QuoteCount",
                table: "PostReport",
                newName: "XQuoteCount");

            migrationBuilder.RenameColumn(
                name: "LikeCount",
                table: "PostReport",
                newName: "XLikeCount");

            migrationBuilder.RenameColumn(
                name: "ImpressionCount",
                table: "PostReport",
                newName: "XImpressionCount");

            migrationBuilder.RenameColumn(
                name: "BookmarkCount",
                table: "PostReport",
                newName: "XBookmarkCount");

            migrationBuilder.RenameColumn(
                name: "TwitterScore",
                table: "HashtagReport",
                newName: "XScore");

            migrationBuilder.RenameColumn(
                name: "TwitterCTR",
                table: "HashtagReport",
                newName: "XCTR");

            migrationBuilder.RenameColumn(
                name: "TwitterReferralCount",
                table: "AttractionReport",
                newName: "XReferralCount");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TourTemplateReport",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PostReport",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TourTemplateReport");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PostReport");

            migrationBuilder.RenameColumn(
                name: "XScore",
                table: "TourTemplateReport",
                newName: "TwitterScore");

            migrationBuilder.RenameColumn(
                name: "XRetweetCount",
                table: "TourTemplateReport",
                newName: "TwitterReferralCount");

            migrationBuilder.RenameColumn(
                name: "XReplyCount",
                table: "TourTemplateReport",
                newName: "RetweetCount");

            migrationBuilder.RenameColumn(
                name: "XReferralCount",
                table: "TourTemplateReport",
                newName: "ReplyCount");

            migrationBuilder.RenameColumn(
                name: "XQuoteCount",
                table: "TourTemplateReport",
                newName: "QuoteCount");

            migrationBuilder.RenameColumn(
                name: "XLikeCount",
                table: "TourTemplateReport",
                newName: "LikeCount");

            migrationBuilder.RenameColumn(
                name: "XImpressionCount",
                table: "TourTemplateReport",
                newName: "ImpressionCount");

            migrationBuilder.RenameColumn(
                name: "XCTR",
                table: "TourTemplateReport",
                newName: "TwitterCTR");

            migrationBuilder.RenameColumn(
                name: "XBookmarkCount",
                table: "TourTemplateReport",
                newName: "BookmarkCount");

            migrationBuilder.RenameColumn(
                name: "XScore",
                table: "PostReport",
                newName: "TwitterScore");

            migrationBuilder.RenameColumn(
                name: "XRetweetCount",
                table: "PostReport",
                newName: "TwitterReferralCount");

            migrationBuilder.RenameColumn(
                name: "XReplyCount",
                table: "PostReport",
                newName: "RetweetCount");

            migrationBuilder.RenameColumn(
                name: "XReferralCount",
                table: "PostReport",
                newName: "ReplyCount");

            migrationBuilder.RenameColumn(
                name: "XQuoteCount",
                table: "PostReport",
                newName: "QuoteCount");

            migrationBuilder.RenameColumn(
                name: "XLikeCount",
                table: "PostReport",
                newName: "LikeCount");

            migrationBuilder.RenameColumn(
                name: "XImpressionCount",
                table: "PostReport",
                newName: "ImpressionCount");

            migrationBuilder.RenameColumn(
                name: "XCTR",
                table: "PostReport",
                newName: "TwitterCTR");

            migrationBuilder.RenameColumn(
                name: "XBookmarkCount",
                table: "PostReport",
                newName: "BookmarkCount");

            migrationBuilder.RenameColumn(
                name: "XScore",
                table: "HashtagReport",
                newName: "TwitterScore");

            migrationBuilder.RenameColumn(
                name: "XCTR",
                table: "HashtagReport",
                newName: "TwitterCTR");

            migrationBuilder.RenameColumn(
                name: "XReferralCount",
                table: "AttractionReport",
                newName: "TwitterReferralCount");
        }
    }
}
