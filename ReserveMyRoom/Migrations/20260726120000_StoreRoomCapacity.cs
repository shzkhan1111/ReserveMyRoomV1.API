using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using ReserveMyRoom.API.Data;

#nullable disable

namespace ReserveMyRoom.API.Migrations;

[DbContext(typeof(ReserveMyRoomDbContext))]
[Migration("20260726120000_StoreRoomCapacity")]
public partial class StoreRoomCapacity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropCheckConstraint(
            name: "CK_Bookings_NumberOfGuests",
            table: "Bookings");

        migrationBuilder.AddColumn<int>(
            name: "Capacity",
            table: "Rooms",
            type: "int",
            nullable: false,
            defaultValue: 1);

        
        migrationBuilder.Sql(
            """
            UPDATE [Rooms]
            SET [Capacity] = CASE [RoomType]
                WHEN 0 THEN 1
                WHEN 1 THEN 2
                WHEN 2 THEN 4
                ELSE 1
            END
            """);

        migrationBuilder.AddCheckConstraint(
            name: "CK_Bookings_NumberOfGuests",
            table: "Bookings",
            sql: "[NumberOfGuests] >= 1");

        migrationBuilder.AddCheckConstraint(
            name: "CK_Rooms_Capacity",
            table: "Rooms",
            sql: "[Capacity] >= 1");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropCheckConstraint(
            name: "CK_Bookings_NumberOfGuests",
            table: "Bookings");

        migrationBuilder.DropCheckConstraint(
            name: "CK_Rooms_Capacity",
            table: "Rooms");

        migrationBuilder.DropColumn(
            name: "Capacity",
            table: "Rooms");

        migrationBuilder.AddCheckConstraint(
            name: "CK_Bookings_NumberOfGuests",
            table: "Bookings",
            sql: "[NumberOfGuests] BETWEEN 1 AND 4");
    }
}
