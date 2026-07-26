namespace ReserveMyRoom.API.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string BookingReference { get; set; } = string.Empty;
        public string GuestName { get; set; } = string.Empty;
        public int NumberOfGuests { get; set; }
        // DateOnly is used because bookings are based on dates, not times.
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
    }
}
