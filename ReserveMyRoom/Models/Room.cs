using ReserveMyRoom.API.Enums;
using System.Collections.ObjectModel;

namespace ReserveMyRoom.API.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; } = string.Empty;

        public RoomType RoomType { get; set; }
        public int Capacity => RoomType switch
        {
            RoomType.Single => 1,
            RoomType.Double => 2,
            RoomType.Deluxe => 4,
            _ => throw new InvalidOperationException(
                $"{RoomType} is not a supported room type.")
        };

        public int HotelId { get; set; }

        public Hotel Hotel { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new Collection<Booking>();
    }
}
