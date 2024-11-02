using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietWay.Repository.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingTourist_Booking_TourBookingId",
                table: "BookingTourist");

            migrationBuilder.DropIndex(
                name: "IX_Booking_CustomerId",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "ProvinceName",
                table: "Province",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "TourBookingId",
                table: "BookingTourist",
                newName: "BookingId");

            migrationBuilder.RenameColumn(
                name: "ParticipantId",
                table: "BookingTourist",
                newName: "TouristId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingTourist_TourBookingId",
                table: "BookingTourist",
                newName: "IX_BookingTourist_BookingId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TourPrice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "BookingTourist",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CustomerId_TourId",
                table: "Booking",
                columns: new[] { "CustomerId", "TourId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingTourist_Booking_BookingId",
                table: "BookingTourist",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingTourist_Booking_BookingId",
                table: "BookingTourist");

            migrationBuilder.DropIndex(
                name: "IX_Booking_CustomerId_TourId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TourPrice");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "BookingTourist");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Province",
                newName: "ProvinceName");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "BookingTourist",
                newName: "TourBookingId");

            migrationBuilder.RenameColumn(
                name: "TouristId",
                table: "BookingTourist",
                newName: "ParticipantId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingTourist_BookingId",
                table: "BookingTourist",
                newName: "IX_BookingTourist_TourBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CustomerId",
                table: "Booking",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingTourist_Booking_TourBookingId",
                table: "BookingTourist",
                column: "TourBookingId",
                principalTable: "Booking",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
