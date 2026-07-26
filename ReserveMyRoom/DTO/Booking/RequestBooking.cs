using System.ComponentModel.DataAnnotations;

namespace ReserveMyRoom.API.DTO.Booking
{
    public class RequestBooking
    {
        public int RoomId { get; set; }

        [Required, StringLength(150, MinimumLength = 1)]
        public string GuestName { get; set; } = string.Empty;
        [Range(1, int.MaxValue)]
        public int NumberOfGuests { get; set; }

        public DateOnly CheckInDate { get; set; }

        public DateOnly CheckOutDate { get; set; }
    }
}
