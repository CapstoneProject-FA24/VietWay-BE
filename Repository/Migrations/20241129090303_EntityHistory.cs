using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class EntityHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StartingProvince",
                table: "TourTemplate",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntityHistory_ModifiedBy",
                table: "EntityHistory",
                column: "ModifiedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_EntityHistory_Account_ModifiedBy",
                table: "EntityHistory",
                column: "ModifiedBy",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntityHistory_Account_ModifiedBy",
                table: "EntityHistory");

            migrationBuilder.DropIndex(
                name: "IX_EntityHistory_ModifiedBy",
                table: "EntityHistory");

            migrationBuilder.DropColumn(
                name: "StartingProvince",
                table: "TourTemplate");
        }
    }
}
