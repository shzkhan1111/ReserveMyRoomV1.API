using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ReserveMyRoom.API.Data;

#nullable disable

namespace ReserveMyRoom.API.Migrations;

[DbContext(typeof(ReserveMyRoomDbContext))]
[Migration("20260725120000_HardenBookingConstraints")]
public partial class HardenBookingConstraints : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Bookings_RoomId",
            table: "Bookings");

        migrationBuilder.RenameColumn(
            name: "NumberOfGuest",
            table: "Bookings",
            newName: "NumberOfGuests");

        migrationBuilder.AlterColumn<string>(
            name: "BookingReference",
            table: "Bookings",
            type: "nvarchar(40)",
            maxLength: 40,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(30)",
            oldMaxLength: 30);

        migrationBuilder.AlterColumn<string>(
            name: "Address",
            table: "Hotels",
            type: "nvarchar(250)",
            maxLength: 250,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.CreateIndex(
            name: "IX_Bookings_RoomId_CheckInDate_CheckOutDate",
            table: "Bookings",
            columns: new[] { "RoomId", "CheckInDate", "CheckOutDate" });

        migrationBuilder.AddCheckConstraint(
            name: "CK_Bookings_DateRange",
            table: "Bookings",
            sql: "[CheckOutDate] > [CheckInDate]");

        migrationBuilder.AddCheckConstraint(
            name: "CK_Bookings_NumberOfGuests",
            table: "Bookings",
            sql: "[NumberOfGuests] BETWEEN 1 AND 4");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropCheckConstraint(
            name: "CK_Bookings_DateRange",
            table: "Bookings");

        migrationBuilder.DropCheckConstraint(
            name: "CK_Bookings_NumberOfGuests",
            table: "Bookings");

        migrationBuilder.DropIndex(
            name: "IX_Bookings_RoomId_CheckInDate_CheckOutDate",
            table: "Bookings");

        migrationBuilder.RenameColumn(
            name: "NumberOfGuests",
            table: "Bookings",
            newName: "NumberOfGuest");

        migrationBuilder.AlterColumn<string>(
            name: "BookingReference",
            table: "Bookings",
            type: "nvarchar(30)",
            maxLength: 30,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(40)",
            oldMaxLength: 40);

        migrationBuilder.AlterColumn<string>(
            name: "Address",
            table: "Hotels",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(250)",
            oldMaxLength: 250);

        migrationBuilder.CreateIndex(
            name: "IX_Bookings_RoomId",
            table: "Bookings",
            column: "RoomId");
    }
}
