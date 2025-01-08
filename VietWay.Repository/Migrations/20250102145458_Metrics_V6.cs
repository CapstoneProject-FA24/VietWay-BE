using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Metrics_V6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "XReferralCount",
                table: "PostMetric",
                newName: "NewXReferralCount");

            migrationBuilder.RenameColumn(
                name: "FacebookReferralCount",
                table: "PostMetric",
                newName: "NewFacebookReferralCount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewXReferralCount",
                table: "PostMetric",
                newName: "XReferralCount");

            migrationBuilder.RenameColumn(
                name: "NewFacebookReferralCount",
                table: "PostMetric",
                newName: "FacebookReferralCount");
        }
    }
}
