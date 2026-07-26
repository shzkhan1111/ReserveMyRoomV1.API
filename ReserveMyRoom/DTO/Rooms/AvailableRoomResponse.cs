using ReserveMyRoom.API.Enums;

namespace ReserveMyRoom.API.DTO.Rooms
{
    public class AvailableRoomResponse
    {
        public int HotelId { get; set; }

        public string HotelName { get; set; } = string.Empty;

        public int RoomId { get; set; }

        public string RoomNumber { get; set; } = string.Empty;

        public RoomType RoomType { get; set; }

        public int Capacity { get; set; }
    }
}
