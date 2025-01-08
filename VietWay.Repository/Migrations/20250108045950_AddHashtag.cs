using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddHashtag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttractionHashtag",
                columns: table => new
                {
                    AttractionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HashtagId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionHashtag", x => new { x.AttractionId, x.HashtagId });
                });

            migrationBuilder.CreateTable(
                name: "Hashtag",
                columns: table => new
                {
                    HashtagId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HashtagName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hashtag", x => x.HashtagId);
                });

            migrationBuilder.CreateTable(
                name: "PostHashtag",
                columns: table => new
                {
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HashtagId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostHashtag", x => new { x.PostId, x.HashtagId });
                });

            migrationBuilder.CreateTable(
                name: "TourTemplateHashtag",
                columns: table => new
                {
                    TourTemplateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HashtagId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTemplateHashtag", x => new { x.TourTemplateId, x.HashtagId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionHashtag");

            migrationBuilder.DropTable(
                name: "Hashtag");

            migrationBuilder.DropTable(
                name: "PostHashtag");

            migrationBuilder.DropTable(
                name: "TourTemplateHashtag");
        }
    }
}
