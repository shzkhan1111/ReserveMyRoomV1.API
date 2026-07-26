using ReserveMyRoom.API.Enums;
using ReserveMyRoom.API.Models;

namespace ReserveMyRoom.API.Data;

public static class DatabaseSeeder
{
    public static void Seed(ReserveMyRoomDbContext context)
    {
        if (context.Hotels.Any())
        {
            return;
        }

        context.Hotels.AddRange(
            CreateHotel("7 Hovik Place", "London"),
            CreateHotel("5 Star Hotel", "Canada"),
            CreateHotel("Rosewood", "Bangkok"),
            CreateHotel("Cappella Sydney", "Sydney"),
            CreateHotel("Bulgari", "Tokyo"));

        context.SaveChanges();
    }

    private static Hotel CreateHotel(string name, string address)
    {
        return new Hotel
        {
            Name = name,
            Address = address,
            Rooms =
            [   
                CreateRoom("101", RoomType.Single, 1),
                CreateRoom("102", RoomType.Single, 2),
                CreateRoom("201", RoomType.Double, 2),
                CreateRoom("202", RoomType.Double, 3),
                CreateRoom("301", RoomType.Deluxe, 4),
                CreateRoom("302", RoomType.Deluxe, 6)
            ]
        };
    }

    private static Room CreateRoom(
        string roomNumber,
        RoomType roomType,
        int capacity)
    {
        return new Room
        {
            RoomNumber = roomNumber,
            RoomType = roomType,
            Capacity = capacity
        };
    }
}
