using ReserveMyRoom.API.Enums;

namespace ReserveMyRoom.API.DTO.Booking
{
    public class BookingResponse
    {
        public string BookingReference { get; set; } = string.Empty;

        public string GuestName { get; set; } = string.Empty;

        public int NumberOfGuests { get; set; }

        public DateOnly CheckInDate { get; set; }

        public DateOnly CheckOutDate { get; set; }

        public string HotelName { get; set; } = string.Empty;

        public string RoomNumber { get; set; } = string.Empty;

        public RoomType RoomType { get; set; }
    }
}
