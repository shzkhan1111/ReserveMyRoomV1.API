using ReserveMyRoom.API.Enums;
using ReserveMyRoom.API.Models;

namespace ReserveMyRoom.API.Data
{
    public static class DatabaseSeeder
    {
        public static void Seed(ReserveMyRoomDbContext context)
        {
            //Database Already Seeded
            if (context.Hotels.Any())
            {
                return;
            }

            context.Hotels.AddRange([
                new Hotel
                    {
                        Name = "7 Hovik Place",
                        Address = "London",
                        Rooms =
                                [
                                    new Room
                                    {
                                        RoomNumber = "101",
                                        RoomType = RoomType.Single
                                    },
                                    new Room
                                    {
                                        RoomNumber = "102",
                                        RoomType = RoomType.Single
                                    },
                                    new Room
                                    {
                                        RoomNumber = "201",
                                        RoomType = RoomType.Double
                                    },
                                    new Room
                                    {
                                        RoomNumber = "202",
                                        RoomType = RoomType.Double
                                    },
                                    new Room
                                    {
                                        RoomNumber = "301",
                                        RoomType = RoomType.Deluxe
                                    },
                                    new Room
                                    {
                                        RoomNumber = "302",
                                        RoomType = RoomType.Deluxe
                                    }
                                ]
                    },
                new Hotel
                    {
                        Name = "5 Star Hotel",
                        Address = "Canada",
                        Rooms =
    [
        new Room
        {
            RoomNumber = "101",
            RoomType = RoomType.Single
        },
        new Room
        {
            RoomNumber = "102",
            RoomType = RoomType.Single
        },
        new Room
        {
            RoomNumber = "201",
            RoomType = RoomType.Double
        },
        new Room
        {
            RoomNumber = "202",
            RoomType = RoomType.Double
        },
        new Room
        {
            RoomNumber = "301",
            RoomType = RoomType.Deluxe
        },
        new Room
        {
            RoomNumber = "302",
            RoomType = RoomType.Deluxe
        }
    ]
                    },
                    new Hotel{
                        Name = "Rosewood",
                        Address = "Bangkok",
                        Rooms =
    [
        new Room
        {
            RoomNumber = "101",
            RoomType = RoomType.Single
        },
        new Room
        {
            RoomNumber = "102",
            RoomType = RoomType.Single
        },
        new Room
        {
            RoomNumber = "201",
            RoomType = RoomType.Double
        },
        new Room
        {
            RoomNumber = "202",
            RoomType = RoomType.Double
        },
        new Room
        {
            RoomNumber = "301",
            RoomType = RoomType.Deluxe
        },
        new Room
        {
            RoomNumber = "302",
            RoomType = RoomType.Deluxe
        }
    ]
                    },
                    new Hotel{
                        Name="Cappella Sydney",
                        Address = "Sydney",
                        Rooms =
    [
        new Room
        {
            RoomNumber = "101",
            RoomType = RoomType.Single
        },
        new Room
        {
            RoomNumber = "102",
            RoomType = RoomType.Single
        },
        new Room
        {
            RoomNumber = "201",
            RoomType = RoomType.Double
        },
        new Room
        {
            RoomNumber = "202",
            RoomType = RoomType.Double
        },
        new Room
        {
            RoomNumber = "301",
            RoomType = RoomType.Deluxe
        },
        new Room
        {
            RoomNumber = "302",
            RoomType = RoomType.Deluxe
        }
    ]
                    },
                    new Hotel{
                        Name="Bulgari",
                        Address = "Tokyo",
                        Rooms =
    [
        new Room
        {
            RoomNumber = "101",
            RoomType = RoomType.Single
        },
        new Room
        {
            RoomNumber = "102",
            RoomType = RoomType.Single
        },
        new Room
        {
            RoomNumber = "201",
            RoomType = RoomType.Double
        },
        new Room
        {
            RoomNumber = "202",
            RoomType = RoomType.Double
        },
        new Room
        {
            RoomNumber = "301",
            RoomType = RoomType.Deluxe
        },
        new Room
        {
            RoomNumber = "302",
            RoomType = RoomType.Deluxe
        }
    ]
                    }
                ]);
            context.SaveChanges();
        }
    }
}
