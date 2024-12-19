using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class removeNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntityHistory_Account_ModifiedBy",
                table: "EntityHistory");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "EntityHistory",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityHistory_Account_ModifiedBy",
                table: "EntityHistory",
                column: "ModifiedBy",
                principalTable: "Account",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntityHistory_Account_ModifiedBy",
                table: "EntityHistory");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "EntityHistory",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityHistory_Account_ModifiedBy",
                table: "EntityHistory",
                column: "ModifiedBy",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
