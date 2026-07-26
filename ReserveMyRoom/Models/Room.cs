using ReserveMyRoom.API.Enums;
using System.Collections.ObjectModel;

namespace ReserveMyRoom.API.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; } = string.Empty;

        public RoomType RoomType { get; set; }

        public int Capacity { get; set; }

        public int HotelId { get; set; }

        public Hotel Hotel { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new Collection<Booking>();
    }
}
